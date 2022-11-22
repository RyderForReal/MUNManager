// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia.Media;
using Config.Net;
namespace MUNManager.Configuration {
	public interface IGlobalConfiguration {
		IBrush BackgroundColor { get; set; }
		IBrush ForegroundColor { get; set; }
		
		bool DebugMode { get; set; }
	}
}