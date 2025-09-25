using NAudio.Wave;

namespace Snake;

public class Resources
{
    public static string ResourcesPath = "";
    private static readonly List<SoundInstance> _sounds = [];

    static Resources()
    {
        var ind = Directory.GetCurrentDirectory().IndexOf("bin", StringComparison.Ordinal);
        var binFolder = Directory.GetCurrentDirectory().Substring(0, ind);
        ResourcesPath = Path.Combine(binFolder, "resources");
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

        player.Init(audio);
        player.Play();
        player.PlaybackStopped += handler;

        _sounds.Add(sound);
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
            return $"{index}# {i.Name}: {i.Score} [{readableTime}]";
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
