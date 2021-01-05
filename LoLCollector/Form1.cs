using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLCollector
{
    public partial class Form1 : Form
    {
        int nMatches = 3;
        int nSummoners = 10;
        public Form1()
        {
            InitializeComponent();
            nMatches = (int)numericUpDown1.Value;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string dummyFileName = "LoL.csv";
            string csvPath = "";
            SaveFileDialog sf = new SaveFileDialog();
            // Feed the dummy name to the save dialog
            sf.FileName = dummyFileName;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                // Now here's our save folder
                csvPath = sf.FileName;
                // Do whatever
            }
            else
            {
                return;
            }
            string apiKey = textBoxApiKey.Text.Trim();
            string summonerName = textBoxSummonerName.Text.Trim(); 
            if((apiKey=="") || (summonerName == ""))
            {
                return;
            }

            Summoner summoner = RiotAPI.getSummoner(summonerName, apiKey);
            MatchList matches = RiotAPI.getMatchList(summoner.accountId, apiKey);
            Match[] match = new Match[nMatches];
            int count=0;
            do
            {
                match[count] = RiotAPI.getMatch(matches.matches[count].gameId.ToString(), apiKey);
                count++;
            } while (count < nMatches);

            //string result = match[2].participantIdentities[0].player.summonerName+" "+ match[2].participants[0].stats.deaths.ToString();



            //build csv
            var csv = new StringBuilder();
            string newline = "";
            newline += "summonerName" + ",";
            newline += "kills" + ",";
            newline += "deaths" + ",";
            newline += "assists" + ",";
            newline += "win" + ",";
            newline += "totalMinionsKilled" + ",";
            newline += "neutralMinionsKilled" + ",";
            newline += "neutralMinionsKilledEnemyJungle" + ",";
            newline += "neutralMinionsKilledTeamJungle" + ",";
            newline += "goldEarned" + ",";
            newline += "visionScore" + ",";
            newline += "largestKillingSpree" + ",";
            newline += "totalDamageDealt" + ",";
            newline += "totalDamageTaken" + ",";
            newline += "teamFirstBaron,";
            newline += "teamBaronKills,";
            newline += "teamFirstDragon,";
            newline += "teamDragonKills,";
            newline += "teamFirstTower,";
            newline += "teamFirstInhibitor,";
            newline += "firstBlood,";
            newline += "firstRiftHerald,";
            newline += "riftHeraldKills,";
            newline += "teamId" + ",";
            newline += "gameId" + ",";
            newline += "gameDuration";
            csv.AppendLine(newline);
            newline = "";
            foreach (Match m in match)
            {
                count = 0;
                do
                {
                    newline += m.participantIdentities[count].player.summonerName + ",";
                    newline += m.participants[count].stats.kills.ToString() + ",";
                    newline += m.participants[count].stats.deaths.ToString() + ",";
                    newline += m.participants[count].stats.assists.ToString() + ",";
                    newline += m.participants[count].stats.win.ToString() + ",";
                    newline += m.participants[count].stats.totalMinionsKilled.ToString() + ",";
                    newline += m.participants[count].stats.neutralMinionsKilled.ToString() + ",";
                    newline += m.participants[count].stats.neutralMinionsKilledEnemyJungle.ToString() + ",";
                    newline += m.participants[count].stats.neutralMinionsKilledTeamJungle.ToString() + ",";
                    newline += m.participants[count].stats.goldEarned.ToString() + ",";
                    newline += m.participants[count].stats.visionScore.ToString() + ",";
                    newline += m.participants[count].stats.largestKillingSpree.ToString() + ",";
                    newline += m.participants[count].stats.totalDamageDealt.ToString() + ",";
                    newline += m.participants[count].stats.totalDamageTaken.ToString() + ",";
                    if(m.teams[0].teamId == m.participants[count].teamId)
                    {
                        newline += m.teams[0].firstBaron.ToString() + ",";
                        newline += m.teams[0].baronKills.ToString() + ",";
                        newline += m.teams[0].firstDragon.ToString() + ",";
                        newline += m.teams[0].dragonKills.ToString() + ",";
                        newline += m.teams[0].firstTower.ToString() + ",";
                        newline += m.teams[0].firstInhibitor.ToString() + ",";
                        newline += m.teams[0].firstBlood.ToString() + ",";
                        newline += m.teams[0].firstRiftHerald.ToString() + ",";
                        newline += m.teams[0].riftHeraldKills.ToString() + ",";
                    }
                    else
                    {
                        newline += m.teams[1].firstBaron.ToString() + ",";
                        newline += m.teams[1].baronKills.ToString() + ",";
                        newline += m.teams[1].firstDragon.ToString() + ",";
                        newline += m.teams[1].dragonKills.ToString() + ",";
                        newline += m.teams[1].firstTower.ToString() + ",";
                        newline += m.teams[1].firstInhibitor.ToString() + ",";
                        newline += m.teams[1].firstBlood.ToString() + ",";
                        newline += m.teams[1].firstRiftHerald.ToString() + ",";
                        newline += m.teams[1].riftHeraldKills.ToString() + ",";
                    }
                    newline += m.participants[count].teamId.ToString() + ",";
                    newline += m.gameId.ToString() + ",";
                    newline += m.gameDuration.ToString();
                    csv.AppendLine(newline);
                    newline = "";
                    count++;
                } while (count < nSummoners);
                
            }

            try
            {
                File.WriteAllText(csvPath, csv.ToString());
                MessageBox.Show("File created", "Done", MessageBoxButtons.OK);
            }
            catch
            {
                MessageBox.Show("Impossibile scrivere il file", "Error", MessageBoxButtons.OK);
            }
            


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            nMatches = (int)numericUpDown1.Value;
        }
    }
}
