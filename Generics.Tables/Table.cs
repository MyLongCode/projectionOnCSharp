using Newtonsoft.Json.Linq;

namespace Generics.Tables;

public class Table<TRow, TCol, TVal>
{
    public Dictionary<TRow, Dictionary<TCol, TVal>> Dict;
    public HashSet<TRow> Rows;
    public HashSet<TCol> Columns;

    public Table()
    {
        Dict = new Dictionary<TRow, Dictionary<TCol, TVal>>();
        Rows = new HashSet<TRow>();
        Columns = new HashSet<TCol>();
    }

    public Table<TRow, TCol, TVal> Open => this;

    public TableExist<TRow, TCol, TVal> Existed => new TableExist<TRow, TCol, TVal>(this);

    public TVal this[TRow row, TCol col]
    {
        get
        {
            if (!Dict.ContainsKey(row) || !Dict[row].ContainsKey(col)) return default(TVal);
            return Dict[row][col];
        }
        set
        {
            if (value.GetType() != typeof(TVal)) throw new ArgumentException();
            if (!Rows.Contains(row)) AddRow(row);
            if (!Columns.Contains(col)) AddColumn(col);
            Dict[row].Add(col,value);
        }
    }

    public bool Exists(TRow row, TCol col) => Rows.Contains(row) && Columns.Contains(col);

    public void AddRow(TRow row)
    {
        Rows.Add(row);
        if (!Dict.ContainsKey(row)) Dict.Add(row, new Dictionary<TCol, TVal>());
    }

    public void AddColumn(TCol col) => Columns.Add(col);
}
public class TableExist<TRow, TCol, TValue>
{
    private Table<TRow, TCol, TValue> table;

    public TValue this[TRow row, TCol col]
    {
        get
        {
            if (!table.Exists(row, col)) throw new ArgumentException();
            return table[row, col];
        }
        set
        {
            if (!table.Exists(row, col) || value.GetType() != typeof(TValue)) throw new ArgumentException();
            table[row, col] = value;
        }
    }

    public TableExist(Table<TRow, TCol, TValue> table) => this.table = table;
}