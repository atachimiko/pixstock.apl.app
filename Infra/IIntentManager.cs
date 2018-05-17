namespace pixstock.apl.app.core.Infra
{
    public interface IIntentManager
    {
        /// <summary>
        /// Intent処理キューに、インテントを追加する
        /// </summary>
        /// <param name="intentName">インテント名</param>
        void AddIntent(string intentName);
    }
}
