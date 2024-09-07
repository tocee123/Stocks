namespace Stocks.DividendUpdater.Setup;
public class ProcessSummary
{
    private int _isDeletedUpdatedCount = 0;
    private int _newDividendPaymentsCount = 0;
    private int _newPriceCount = 0;
    private bool _isStarted = false;

    public void IncreaseIsDeletedUpdated() => Update(() => _isDeletedUpdatedCount++);
    public void AddNewPricesCount(int count) => Update(() => _newDividendPaymentsCount += count);
    public void AddNewDividendPaymentsCount(int count) => Update(() => _newDividendPaymentsCount += count);

    private void Update(Action action)
    {
        action();
        _isStarted = true;
    }

    public bool IsFinished => _isStarted && _isDeletedUpdatedCount == 0 && _newDividendPaymentsCount == 0 && _newPriceCount == 0;

    public override string ToString()
    => $"Summary:\nCount of isDeleted: {_isDeletedUpdatedCount}\nCount of new dividend payments: {_newDividendPaymentsCount}\nCount of new prices: {_newPriceCount}";
}