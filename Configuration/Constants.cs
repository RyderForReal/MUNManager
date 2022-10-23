namespace MUNManager.Configuration {
	public static class Constants {
		// Must be 1 or higher as < operator is used and duration is an unsigned int.
		internal const int UnmoderatedCaucusMinimumDuration = 10;
		internal const int ModeratedCaucusMinimumDurationEach = 10;
		internal const int ModeratedCaucusMinimumDuration = 20;
	}
}