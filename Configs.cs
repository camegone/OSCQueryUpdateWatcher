// From NuGet. run `dotnet add System.Collections.Immutable`.
using System.Collections.Immutable;

namespace camegone.OSCQuery.Listener;

// Configuable constants variables.
static class Configs
{
    // Cannot declare an array as const, so I use ImmutableArray instead.
    public static readonly ImmutableArray<string> AllowedClientPrefixes = ImmutableArray.Create(
        // Write down client name prefixes to read values here...
        // "Hoge-Fuga",
        // "Foo-Bar",
        "VRChat-Client-"
    );

    // Name of this client
    public static readonly string ClientName = "OSCQueryLogger";
    // span of request tree
    public static readonly int ListenDelayMillis = 10;
    public static readonly int TimeoutMillis = 1000;
}
