using NCMB;
using System.Collections.Generic;

public class LeaderBoard
{

    public int currentRank = 0;
    public List<NCMB.HighScore> topRankers = null;
    public List<NCMB.HighScore> neighbors = null;

    // 現プレイヤーのハイスコアを受けとってランクを取得 ---------------
    public void fetchRank(int currentScore)
    {
        // データスコアの「HighScore」から検索
        NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject>("HighScore");
        rankQuery.WhereGreaterThan("Score", currentScore);
        rankQuery.CountAsync((int count, NCMBException e) => {

            if (e != null)
            {
                //件数取得失敗
            }
            else
            {
                //件数取得成功
                currentRank = count + 1; // 自分よりスコアが上の人がn人いたら自分はn+1位
            }
        });
    }

    // サーバーからトップ(Limit件)を取得 ---------------    
    public void fetchTopRankers(int limit)
    {
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
        query.OrderByDescending("Score");
        query.Limit = limit;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                List<NCMB.HighScore> list = new List<NCMB.HighScore>();
                // 取得したレコードをHighScoreクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    int s = System.Convert.ToInt32(obj["Score"]);
                    string n = System.Convert.ToString(obj["Name"]);
                    list.Add(new HighScore(s, n));
                }
                topRankers = list;
            }
        });
    }

    // サーバーからrankの前後Limit件を取得 ---------------
    public void fetchNeighbors(int limit) //自分のランクの前後count人ずつ取得
    {
        neighbors = new List<NCMB.HighScore>();

        // スキップする数を決める（ただし自分が1位か2位のときは調整する）
        int numSkip = currentRank - (limit / 2);
        if (numSkip < 0) numSkip = 0;

        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
        query.OrderByDescending("Score");
        query.Skip = numSkip;
        query.Limit = limit;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                List<NCMB.HighScore> list = new List<NCMB.HighScore>();
                // 取得したレコードをHighScoreクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    int s = System.Convert.ToInt32(obj["Score"]);
                    string n = System.Convert.ToString(obj["Name"]);
                    list.Add(new HighScore(s, n));
                }
                neighbors = list;
            }
        });
    }
}