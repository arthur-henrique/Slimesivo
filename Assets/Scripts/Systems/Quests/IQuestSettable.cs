public interface IQuestSettable
{
    bool CompletedQuest(string dicKey, int keyInt);
    void SetQuestCompleteToPrefs(string dicKey, int keyInt);
}
