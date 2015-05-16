using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Text;

// Goog lePlayGamesプラグインを入れたら有効にする
#if false

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class GameSocial : MonoBehaviour
{
	public bool IsAuthenticated{ get; private set; }
    [SerializeField] bool exceptHyphenByIOS = true;
    private Dictionary<string, string> mLeaderboards;
	private Dictionary<string, string> mAchievements;

    private string[] mLeaderboardKeys;
	public string[] LeaderboardKeys
	{
		get
		{
			return mLeaderboardKeys;
		}
	}

    private string[] mAchievementKeys;
	public string[] AchievementKeys
	{
		get
		{
			return mAchievementKeys;
		}
	}

    public bool IsLoaded {get; private set;}

	[SerializeField] private TextAsset LeaderboardsCSV;
	[SerializeField] private TextAsset AchievementsCSV;

	static public GameSocial Instance {get; private set;}

	public void SaveLeaderboard(int index, long score)
	{
		SaveLeaderboard(mLeaderboardKeys[index], score);
	}

    public void SaveLeaderboard(string key, float second)
    {
#if UNITY_ANDROID
        SaveLeaderboard(key, (long)(1000 * second));
#else
        SaveLeaderboard(key, (long)(100 * second));
#endif
    }

	public void SaveLeaderboard(string key, long score)
	{
		Debug.Log ("SaveLeaderboard:" + key + "(" + mLeaderboards[key] + "):" + score);
        if (!IsAuthenticated) 
        {
            return;
        }
#if !UNITY_EDITOR
        Social.ReportScore(score, GetKey(mLeaderboards[key]), (success) => 
		{
		    Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
#endif
	}

	public void SaveAchievement(int index, double progress = 100.0)
	{
		SaveAchievement(mAchievementKeys[index], progress);
	}

	public void SaveAchievement(string key, double progress = 100.0)
	{
		Debug.Log("Save Achievement: key=" + key + ": progress=" + progress);
        if (!IsAuthenticated) 
        {
            return;
        }
#if !UNITY_EDITOR
        Social.ReportProgress(GetKey(mAchievements[key]), progress, (success) => 
		{
			Debug.Log(success ? "Reported progress successfully" : "Failed to report progress");
		});
#endif
	}

    private string GetKey(string originalKey)
    {
#if UNITY_IOS
        if (exceptHyphenByIOS)
        {
            return originalKey.Replace("-", "");
        }
#endif
        return originalKey;
    }

	void Awake()
	{
        IsLoaded = false;

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	void Start()
	{
        ReadCsv(LeaderboardsCSV.text, ref mLeaderboardKeys, ref mLeaderboards);
        ReadCsv(AchievementsCSV.text, ref mAchievementKeys, ref mAchievements);
	}

    void ReadCsv(string text, ref string[] keys, ref Dictionary<string, string> dic)
    {
        var csv = CSVReader.SplitCsvGrid(text);
        dic = new Dictionary<string, string>();
        for(int i=0 ; i<csv.GetUpperBound(1) ; ++i)
        {
            if (csv[0, i] != null)
            {
                dic.Add(csv[0, i], csv[1, i]);
            }
        }
        keys = new string[dic.Count];
        dic.Keys.CopyTo(keys, 0);
    }

    public void Init(System.Action<bool> callback=null)
    {
        if (!IsAuthenticated) 
        {
            Authenticate(callback);
        }
        else
        {
            if (callback != null)
            {
                callback(true);
            }
        }
	}

    public void ShowLeaderBoard(System.Action<bool> callback = null)
    {
        if (IsLoaded)
        {
            if (IsAuthenticated) 
            {
                Social.ShowLeaderboardUI();
                if (callback != null)
                {
                    callback(true);
                }
            }
            else 
            {
                Authenticate((success) => {
                    Social.ShowLeaderboardUI();
                    if (callback != null)
                    {
                        callback(success);
                    }
                });
            }
        }
        else
        {
            if (callback != null)
            {
                callback(false);
            }
        }
    }

    public void ShowAchievement(System.Action<bool> callback = null)
    {
        if (IsLoaded)
        {
            if (IsAuthenticated) 
            {
                Social.ShowAchievementsUI();
                if (callback != null)
                {
                    callback(true);
                }
            }
            else 
            {
                Authenticate((success) => {
                    Social.ShowAchievementsUI();
                    if (callback != null)
                    {
                        callback(success);
                    }
                });
            }
        }
        else
        {
            if (callback != null)
            {
                callback(false);
            }
        }
    }

	void Authenticate(System.Action<bool> callback=null)
	{

#if !UNITY_EDITOR

#if UNITY_ANDROID
	    PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
#endif
        IsLoaded = false;
		Social.localUser.Authenticate((success) => 
		{ 
			Debug.Log ("Authenticate:" + success.ToString());
			if (!success) {
				Debug.Log ("####ERROR");
			}
			if (callback != null) {
				callback(success);
			}
            IsAuthenticated = success;
            IsLoaded = true;
		});
#else
        if (callback != null) {
            callback(true);
        }
        IsLoaded = true;

#endif

	}
}
#endif

