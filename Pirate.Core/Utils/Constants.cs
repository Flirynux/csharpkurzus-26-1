using Pirate.Core.entities;

namespace Pirate.Core.Utils;

static class Constants
{
    public static int MAP_WIDTH = 720;
    public static int MAP_HEIGHT = 450;
    public static int VERTICAL_SCALE = 2;
    public static int DRAW_WIDTH = Console.WindowWidth;
    public static int DRAW_HEIGHT = (Console.WindowHeight-1)* VERTICAL_SCALE;
    public static int PLAYER_MOVEMENT_BORDER_LEFT = DRAW_WIDTH/2 + 1;
    public static int PLAYER_MOVEMENT_BORDER_RIGHT = MAP_WIDTH-(DRAW_WIDTH/2 + 1);
    public static int PLAYER_MOVEMENT_BORDER_TOP = DRAW_HEIGHT/2 + 1;
    public static int PLAYER_MOVEMENT_BORDER_BOTTOM = MAP_HEIGHT-(DRAW_HEIGHT/2 + 1);
}
