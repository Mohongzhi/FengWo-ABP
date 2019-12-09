using System.Collections.Generic;

namespace FengWo.Configuration.Ui
{
    public static class UiThemes
    {
        public static List<UiThemeInfo> All { get; }

        static UiThemes()
        {
            All = new List<UiThemeInfo>
            {
                new UiThemeInfo("Cyan", "cyan"),
                new UiThemeInfo("Red", "red"),
                new UiThemeInfo("Pink", "pink"),
                new UiThemeInfo("Purple", "purple"),
                new UiThemeInfo("DeepPurple", "deep-purple"),
                new UiThemeInfo("Indigo", "indigo"),
                new UiThemeInfo("Blue", "blue"),
                new UiThemeInfo("LightBlue", "light-blue"),
                new UiThemeInfo("Teal", "teal"),
                new UiThemeInfo("Green", "green"),
                new UiThemeInfo("LightGreen", "light-green"),
                new UiThemeInfo("Lime", "lime"),
                new UiThemeInfo("Yellow", "yellow"),
                new UiThemeInfo("Amber", "amber"),
                new UiThemeInfo("Orange", "orange"),
                new UiThemeInfo("DeepOrange", "deep-orange"),
                new UiThemeInfo("Brown", "brown"),
                new UiThemeInfo("Grey", "grey"),
                new UiThemeInfo("BlueGrey", "blue-grey"),
                new UiThemeInfo("Black", "black")
            };
        }
    }
}
