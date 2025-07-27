using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace ImaginedWorlds.Infrastructure;

public static class JsonNodeHelpers
{
    public static void SetValueByPath(JsonObject root, string path, JsonNode value)
    {
        string[] segments = path.Split('.');
        JsonNode currentNode = root;

        for (int i = 0; i < segments.Length - 1; i++)
        {
            (string name, int? index) = ParseSegment(segments[i]);

            JsonNode? nextNode = (index.HasValue)
                ? currentNode[name]?[index.Value]
                : currentNode[name];

            if (nextNode is null)
            {
                // Look ahead to see if the next segment is an array or object.
                (string nextName, int? nextIndex) = ParseSegment(segments[i + 1]);
                nextNode = nextIndex.HasValue ? new JsonArray() : new JsonObject();

                if (index.HasValue)
                {
                    if (currentNode[name] is not JsonArray array)
                    {
                        array = new JsonArray();
                        currentNode[name] = array;
                    }
                    // Ensure array is large enough
                    while (array.Count <= index.Value) array.Add(null);
                    array[index.Value] = nextNode;
                }
                else
                {
                    currentNode[name] = nextNode;
                }
            }
            currentNode = nextNode;
        }

        (string finalName, int? finalIndex) = ParseSegment(segments[^1]);
        if (finalIndex.HasValue)
        {
            if (currentNode[finalName] is not JsonArray array)
            {
                array = new JsonArray();
                currentNode[finalName] = array;
            }
            while (array.Count <= finalIndex.Value) array.Add(null);
            array[finalIndex.Value] = value;
        }
        else
        {
            currentNode[finalName] = value;
        }
    }

    private static (string Name, int? Index) ParseSegment(string segment)
    {
        Match match = Regex.Match(segment, @"(\w+)\[(\d+)\]");
        if (match.Success)
        {
            return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
        }
        return (segment, null);
    }
}
