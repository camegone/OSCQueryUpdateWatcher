// From NuGet. run `dotnet add System.Collections.Immutable`.
using System.Collections.Immutable;
using System.Threading.Tasks;
using VRC.OSCQuery;

using static System.Console;

namespace camegone.OSCQuery.Listener;

class OSCListener : IDisposable
{
    // TODO: Handle the case of multiple target exsists

    // Store target client names in Immutable way to avoid corruptions.
    public ImmutableArray<string> TargetNames { get; protected set; }
    // this show profile currentry watching
    public OSCQueryServiceProfile? TargetProfile {
        get => _targetProfile;
        protected set
        {
            if (_targetProfile != value)
                OnTargetChanged?.Invoke();
            _targetProfile = value;
        }
    }
    public event Action? OnTargetChanged;
    // the nodes of the watched tree
    public OSCQueryRootNode LatestTree
    {
        get => _latestTree;
        protected set
        {
            if ($"{value}" != $"{_latestTree}")
                OnTreeChanged?.Invoke();
            _latestTree = value;
        }
    }
    public event Action? OnTreeChanged;
    // This service shold be explicitly disposed, and should not be started more than once
    private static readonly OSCQueryService _service;
    protected OSCQueryRootNode _latestTree;
    protected OSCQueryServiceProfile? _targetProfile;

    static OSCListener()
    {
        // Get random 32bit hex string
        string randPath = new Random().Next().ToString("X");
        // Resister service
        _service = new OSCQueryServiceBuilder()
            // First, modify class' properties.
            .WithTcpPort(Extensions.GetAvailableTcpPort())
            .WithUdpPort(Extensions.GetAvailableUdpPort())
            .WithServiceName($"{Configs.ClientName}-Client-{randPath}")
            // Then activate server with this function
            .WithDefaults()
            .Build();
        WriteLine($"{_service.ServerName} has been started at Tcp:{_service.TcpPort}, OSC:{_service.OscPort}");
    }

    public OSCListener(ImmutableArray<string> targetNames)
    {
        TargetNames = targetNames;
        LatestTree = _service.RootNode;
        FindClient();
    }
    public OSCListener(string[] targetNames) : this(targetNames.ToImmutableArray()){}
    public OSCListener(string targetName) : this(ImmutableArray.Create(targetName)){}

    private void FindClient()
    {
        _service.OnOscQueryServiceAdded += (serviceProfile) =>
        {
            WriteLine($"OSCQuery Service Found: {serviceProfile.name} {serviceProfile.address}:{serviceProfile.port}");
            if (IsAllowedName(serviceProfile.name))
            {
                WriteLine($"Target has been set to {serviceProfile.name}");
                TargetProfile = serviceProfile;
            }
        };
    }
    private bool IsAllowedName(string? name)
    {
        if (name != null)
        {
            foreach (var a in TargetNames)
            {
                if (name.StartsWith(a))
                    return true;
            }
        }
        return false;
    }
    public async Task RequestTree()
    {
        if (TargetProfile == null)
            return;
        // TODO: Imprement TimeOut
        // Try to get tree.
        LatestTree = await Extensions.GetOSCTree(TargetProfile.address, TargetProfile.port);
    }

    public OSCQueryNode? GetNodeWithPath(string path)
        => LatestTree.GetNodeWithPath(path);
    
    public bool TryGetNodeValue<T>(string path, out T? value, out string nodeType)
        => NodeReader.TryGetNodeValue(this.GetNodeWithPath(path), out value, out nodeType);
    public bool TryGetNodeValue<T>(string path, out T? value)
        => TryGetNodeValue(path, out value, out _);
    
    // Imprement IDisposable.
    public void Dispose()
    {
        _service?.Dispose();
    }

    ~OSCListener()
    {
        this.Dispose();
    }
}