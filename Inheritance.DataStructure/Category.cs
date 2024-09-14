using System.Collections;

namespace Inheritance.DataStructure;

class Category : IComparable
{
    public string Name { get; set; }
    public MessageType MessageType { get; set; }
    public MessageTopic MessageTopic { get; set; }

    public Category(string name, MessageType messageType, MessageTopic messageTopic)
    {
        Name = name;
        MessageType = messageType;
        MessageTopic = messageTopic;
    }

    public static bool operator ==(Category c1, Category c2)
    {
        return c1.CompareTo(c2) == 0;
    }
    public static bool operator !=(Category c1, Category c2)
    {
        return c1.CompareTo(c2) != 0;
    }
    public static bool operator <(Category c1, Category c2)
    {
        return c1.CompareTo(c2) < 0;
    }
    public static bool operator >(Category c1, Category c2)
    {
        return c1.CompareTo(c2) > 0;
    }
    public static bool operator <=(Category c1, Category c2)
    {
        return c1.CompareTo(c2) <= 0;
    }
    public static bool operator >=(Category c1, Category c2)
    {
        return c1.CompareTo(c2) >= 0;
    }

    public bool Equals(Category other)
    {
        if (other is null) return false;
        if (this.Name == other.Name 
            && this.MessageTopic == other.MessageTopic 
            && this.MessageType == other.MessageType) return true;
        return false;
    }
    public virtual int GetHashCode()
    {
        int total = 0;
        foreach (var item in this.Name)
            total += 7 * (int)item;
        total += 11 * (int)this.MessageType;
        total += 13 * (int)this.MessageTopic;
        return total;
    }
    public string ToString()
    {
        return $"{this.Name}.{this.MessageType}.{this.MessageTopic}";
    }
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        Category other = obj as Category;
        if (this.Name is null || other.Name is null) return 1;
        if (this.Name == other.Name)
        {
            if (this.MessageType == other.MessageType)
            {
                if (this.MessageTopic == other.MessageTopic)
                    return 0;
                else if (this.MessageTopic > other.MessageTopic) return 1;
                else return -1;
            }
            else if (this.MessageType > other.MessageType) return 1;
            else return -1;
        }
        return this.Name.CompareTo(other.Name);
    }
}