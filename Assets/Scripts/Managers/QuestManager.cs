using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private QuestManager instance;
    public QuestManager Instance
    {
        get { return instance; }
    }

    public List<Quest> quests;
    
    public void ActivateQuest(string questName, int coinReward)
    {
        Quest newQuest = new Quest(questName, coinReward);
        quests.Add(newQuest);
    }
    public void CompleteQuest(string questName)
    {
        for (int i = 0; i < quests.Count; i++) 
        { 
            if (quests[i].name == questName)
            {
                quests[i].isCompleted = true;
                quests.Remove(quests[i]);
                return;
            }
        }
    }
}
