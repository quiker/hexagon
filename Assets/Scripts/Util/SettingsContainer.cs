using UnityEngine;
using System.Collections;

public class SettingsContainer {

	public static bool GetMusicFlag()
	{
		return PlayerPrefs.GetInt("music_flag")!=0;
	}
	
	public static void SetMusicFlag(bool flag)
	{
		PlayerPrefs.SetInt("music_flag", flag?1:0);
	}
	
	public static int GetLevelStars(int level)
	{
		if (level > 0 && level <= 30) {
			return PlayerPrefs.GetInt("level"+level+"_stars");
		}
		return 0;
	}
	
	public static void SetLevelStars(int level, int stars)
	{
		if (level > 0 && level <= 30 && stars > 0 && stars <= 3) {
			int currentStars = PlayerPrefs.GetInt("level"+level+"_stars");
			if (stars > currentStars) {
				PlayerPrefs.SetInt("level"+level+"_stars", stars);
			}
		}
	}
	
	public static int GetLevelMaxScore(int level)
	{
		if (level > 0 && level <= 30) {
			return PlayerPrefs.GetInt("level"+level+"_stars");
		}
		return 0;
	}
	
	public static void SetLevelMaxScore(int level, int score)
	{
		if (level > 0 && level <= 30) {
			int currentMaxScore = PlayerPrefs.GetInt("level"+level+"_max_score");
			if (score > currentMaxScore) {
				PlayerPrefs.SetInt("level"+level+"_max_score", score);
			}
		}
	}
	
	public static string GetScoreTableLabel(int row)
	{
		if (row >= 0 && row < 10) {
			return PlayerPrefs.GetString("score_table_"+row+"_label");
		}
		return "";
	}
	
	public static int GetScoreTableValue(int row)
	{
		if (row >= 0 && row < 10) {
			return PlayerPrefs.GetInt("score_table_"+row+"_value");
		}
		return 0;
	}
}
