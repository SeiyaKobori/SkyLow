using NCMB;
using System.Collections.Generic;

namespace NCMB
{
    public class HighScore
    {
        public int score { get; set; }
        public string name { get; private set; }

        // コンストラクタ -----------------------------------
        public HighScore(int _score, string _name)
        {
            score = _score;
            name = _name;
        }

        // サーバーにハイスコアを保存 -------------------------
        public void save()
        {
            // データストアの「HighScore」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //検索成功したら
                if (e == null)
                {
                    objList[0]["Score"] = score;
                    objList[0].SaveAsync();
                }
            });
        }

        // サーバーからハイスコアを取得  -----------------
        public void fetch()
        {
            // データストアの「HighScore」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //検索成功したら
                if (e == null)
                {
                    // ハイスコアが未登録だったら
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject("HighScore");
                        obj["Name"] = name;
                        obj["Score"] = 0;
                        obj.SaveAsync();
                        score = 0;
                    }
                    // ハイスコアが登録済みだったら
                    else
                    {
                        score = System.Convert.ToInt32(objList[0]["Score"]);
                    }
                }
            });
        }

    }
}