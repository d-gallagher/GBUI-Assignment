public interface IFeedbackController
{
    void Shake(Enums.FeedbackType feedbackType);
    void ShakeAndVibrate(Enums.FeedbackType feedbackType);
    void Vibrate(Enums.FeedbackType feedbackType);
}