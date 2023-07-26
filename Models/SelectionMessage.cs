namespace tft_cosmetics_manager.Models
{
    public class SelectionMessage<T>
    {
        public T SelectedItem { get; }
        public SelectionMessage(T selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}
