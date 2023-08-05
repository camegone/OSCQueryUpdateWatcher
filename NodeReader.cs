using VRC.OSCQuery;
using static VRC.OSCQuery.Attributes;
using static System.Console;

namespace camegone.OSCQuery.Listener;

static class NodeReader{
    private static bool IsReadable(OSCQueryNode? node)
        => (node != null) && ((node.Access == AccessValues.ReadOnly) || (node.Access == AccessValues.ReadWrite));

    // try to read the value. if success, return true and put out to the value, else return false and new Object. NOTE that this will return false if you have passed invalid arguments.
    public static bool TryGetNodeValue<T>(OSCQueryNode? node, out T? value, out string nodeType)
    {
        // clear a out variable
        value = default;
        nodeType = "";
        // check if noad is readable
        if (IsReadable(node))
        {
            nodeType = node!.OscType;
            try
            {
                // try to get value (node is not null here because of IsReadable() check above)
                dynamic val = node!.Value[0];
                // try to parse value
                // To force cast value to out var, use dynamic variable and exception supressing.
                // Because we cannot assign specified type to generic tipe, we need to cast dynamic var to specified type, then assign to the destination.
                switch (node.OscType)
                {
                    // need to double cast 32bit values to get rid of exception
                    case "i":
                        // int value
                        val = (int)(long)val;
                        value = (T)val;
                        return true;
                    case "u":
                        // unsigned int value
                        val = (uint)(ulong)val;
                        value = (T)val;
                        return true;
                    case "h":
                        // long value
                        val = (long)val;
                        value = (T)val;
                        return true;
                    case "f":
                        // float value
                        val = (float)(double)val;
                        value = (T)val;
                        return true;
                    case "d":
                        // double value
                        val = (double)val;
                        value = (T)val;
                        return true;
                    case "s":
                        // string value
                        val = (string)val;
                        value = (T)val;
                        return true;
                    case "c":
                        // char value
                        val = (char)val;
                        value = (T)val;
                        return true;
                    case "a":
                        // [,] value
                        val = (Array)val;
                        value = (T)val;
                        return true;
                    case "b":
                        // bitarray value
                        val = (byte[])val;
                        value = (T)val;
                        return true;
                    case "T":
                        // bool value
                        val = (bool)val;
                        value = (T)val;
                        return true;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
        }
        // When failed to read. return false and a default value
        return false;
    }
    public static bool TryGetNodeValue<T>(OSCQueryNode? node, out T? value)
        => TryGetNodeValue(node, out value, out _);
}