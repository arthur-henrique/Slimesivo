public interface IQuestSettable
{
    bool CompletedQuest();
    void SetQuestCompleteToPrefs(string dicKey, int keyInt);
}
