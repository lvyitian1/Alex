namespace RocketUI
{
	public interface IScrollable
	{

		bool CanScrollUp(bool alternateDirection = false);
		bool CanScrollDown(bool alternateDirection = false);

		void InvokeScrollUp(bool alternateDirection = false);
		void InvokeScrollDown(bool alternateDirection = false);

	}
}
