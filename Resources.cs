using NAudio.Wave;

namespace Snake;

public class Resources
{
    public static string ResourcesPath = "";
    private static readonly List<SoundInstance> _sounds = [];
    public static readonly Dictionary<string, char[,]> Maps = [];
    public static readonly Dictionary<SkinColor, SnakeColor> SkinColors = [];
    public static readonly Dictionary<SkinSymbol, SnakeSymbol> SkinSymbols = [];

    static Resources()
    {
        var ind = Directory.GetCurrentDirectory().IndexOf("bin", StringComparison.Ordinal);
        var binFolder = Directory.GetCurrentDirectory().Substring(0, ind);
        ResourcesPath = Path.Combine(binFolder, "resources");
        LoadMaps();
        LoadSkins();
    }

    public static void LoadSkins()
    {
        SkinColors.Clear();
        SkinSymbols.Clear();

        var colors = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(SnakeColor).IsAssignableFrom(t) && !t.IsAbstract);
        var symbols = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(SnakeSymbol).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in colors)
        {
            if (Activator.CreateInstance(type) is SnakeColor instance)
            {
                SkinColors[instance.Type] = instance;
            }
        }

        foreach (var type in symbols)
        {
            if (Activator.CreateInstance(type) is SnakeSymbol instance)
            {
                SkinSymbols[instance.Type] = instance;
            }
        }
    }

    public static void LoadMaps()
    {
        var maps = Directory.GetFiles(Path.Combine(ResourcesPath, "maps"));
        Maps.Clear();

        foreach (var mapFile in maps)
        {
            var fileName = Path.GetFileName(mapFile);
            var lines = File.ReadAllLines(mapFile);
            if (lines.Length == 0)
                continue;

            var dim = lines[0].Split(':');
            var width = int.Parse(dim[0]);
            var height = int.Parse(dim[1]);
            var map = Maps[fileName] = new char[width, height];

            for (var y = 0; y < height; y++)
            {
                var line = lines.Length > y ? lines[y + 1] : "";
                for (var x = 0; x < width; x++)
                {
                    if (x >= line.Length)
                    {
                        map[x, y] = ' ';
                        continue;
                    }

                    map[x, y] = line[x];
                }
            }
        }
    }

    public static void RemoveAllSounds()
    {
        foreach (var sound in _sounds)
        {
            // Detach event handler to avoid double-dispose
            if (sound.Handler != null)
                sound.Player.PlaybackStopped -= sound.Handler;

            if (sound.Player.PlaybackState is PlaybackState.Playing or PlaybackState.Paused)
                sound.Player.Stop();

            sound.Player.Dispose();
            sound.Audio.Dispose();
        }
        _sounds.Clear();
    }

    public static void PlaySound(string filePath)
    {
        var audio = new AudioFileReader(Path.Combine(ResourcesPath, filePath));
        var player = new WaveOutEvent();
        var sound = new SoundInstance(player, audio);

        EventHandler<StoppedEventArgs> handler = (s, e) =>
        {
            player.Dispose();
            audio.Dispose();
            _sounds.Remove(sound);
        };
        sound.Handler = handler;

        try
        {
            player.Init(audio);
            player.Play();
            player.PlaybackStopped += handler;

            _sounds.Add(sound);
        }
        catch (Exception ex)
        {
            // ignore
            return;
        }
    }

    public static void SaveScore(string name, int score)
    {
        var scoresFile = Path.Combine(ResourcesPath, "scores.txt");
        long utcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var line = $"{score} | {utcSeconds} | {name}";
        File.AppendAllText(scoresFile, line + Environment.NewLine);
    }

    public static List<string> GetScoreTop5()
    {
        var scoresFile = Path.Combine(ResourcesPath, "scores.txt");
        if (!File.Exists(scoresFile))
        {
            return [];
        }
        var lines = File.ReadAllLines(scoresFile);
        var scores = new List<(int Score, int Time, string Name)>();
        foreach (var line in lines)
        {
            var parts = line.Split('|', StringSplitOptions.TrimEntries);
            if (parts.Length < 3) continue;
            if (!int.TryParse(parts[0], out var score)) continue;
            if (!int.TryParse(parts[1], out var time)) continue;

            scores.Add((score, time, parts[2]));
        }

        var top5 = scores.OrderByDescending(s => s.Score).ThenBy(s => s.Time).Take(5).ToList();

        var index = 0;
        var ret = top5.Select(i => 
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(i.Time).UtcDateTime;
            var readableTime = dateTime.ToString("yyyy-MM-dd");
            index++;
            var str = $"#{index} ";
            str += i.Name.PadRight(8);
            str += $"- {i.Score}".PadRight(6);
            str += $"[{readableTime}]";
            return str;
        });

        return ret.ToList();
    }
}

class SoundInstance
{
    public WaveOutEvent Player { get; }
    public AudioFileReader Audio { get; }
    public EventHandler<StoppedEventArgs>? Handler { get; set; }

    public SoundInstance(WaveOutEvent player, AudioFileReader audio)
    {
        Player = player;
        Audio = audio;
    }
}
