namespace Shared
{
    public static class QueueConfigurations
    {
        public const string RegisterOutboxQueue = "outbox-register";
        public const string MailSentEvent = "mail-sent-event";
        public const string StateMachine = "orchestration-machine";
        public const string Checkpoint_Api_AnalysisNotAvgTime_Identity = "checkpoint-api-analysisnotavgtime-identity-service";
        public const string Identity_Server_UserTeamSelected_Mail_Service = "identity-server-userteamselected-mail-service";
        public const string Checkpoint_Api_ListProject_Identity = "checkpoint-api-listproject-identity-service";
        public const string Identity_GetTeamName_Checkpoint_Api = "identity-service-getteamname-checkpoint-api";
        public const string Identity_Server_TeamNameReceived_Checkpoint_Api = "identity-service-teamnamereceived-checkpoint-api";
    }
}
