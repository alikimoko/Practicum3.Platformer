using System.Collections.Generic;

public class GameSettingsManager
{
    protected Dictionary<string, string> stringSettings;

    /// <summary>Create a new game settings manager.</summary>
    public GameSettingsManager()
    { stringSettings = new Dictionary<string, string>(); }

    /// <summary>Create or set a setting to a value.</summary>
    /// <param name="key">The setting to create or set.</param>
    /// <param name="value">The value to set the setting to.</param>
    public void SetValue(string key, string value)
    { stringSettings[key] = value; }

    /// <summary>Get the value of a given setting</summary>
    /// <param name="key">The setting to get the value of.</param>
    /// <returns>The value the setting contains.</returns>
    public string GetValue(string key)
    {
        try
        {
            return stringSettings[key];
        }   
        catch
        {
            System.Diagnostics.Debug.WriteLine("No setting with the name \"" + key + "\"");
            return "";
        }
    }
}
