namespace Snake;

public class Program
{
    public const int PlaygroundWidth = 50;
    public const int PlaygroundHeight = 25;

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();

            // alg menu kus kasutaja valib mida ta tahab teha
            // me ainult lugeme uks klahv and siis käivitame programmi
            Utils.WriteCentered("Snek Game", PlaygroundWidth / 2, 10);
            Utils.WriteCentered("Press a key to start", PlaygroundWidth / 2, 11);
            Utils.WriteCentered("1 - Play", PlaygroundWidth / 2, 13);
            Utils.WriteCentered("2 - Map Editor", PlaygroundWidth / 2, 15);
            Utils.WriteCentered("3 - Choose Map", PlaygroundWidth / 2, 17);
            Utils.WriteCentered("4 - Choose Skin", PlaygroundWidth / 2, 19);
            var key = Console.ReadKey(true).Key;

            Console.Clear();

            switch (key)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    Menus.Game();
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    Menus.MapEditor();
                    Resources.LoadMaps();
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    Menus.ChooseMap();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    Menus.ChooseSkin();
                    break;
            }

            if (key == ConsoleKey.Escape)
                break;
        } 
    }
}