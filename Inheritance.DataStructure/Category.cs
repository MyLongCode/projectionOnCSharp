namespace Inheritance.DataStructure;

class Category : IComparable<Category>
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


    public int CompareTo(Category other)
    {
        if (this.Name == other.Name)
        {
            if (this.MessageType == other.MessageType)
            {
                if (this.MessageTopic == other.MessageTopic)
                    return 0;
                else if (this.MessageTopic > other.MessageTopic) return -1;
                else return 1;
            }
            else if (this.MessageType > other.MessageType) return -1;
            else return 1;
        }
        return this.Name.CompareTo(other.Name);    
                
    }

    public override bool <

    public bool Equals(Category other)
    {
        if (this.Name == other.Name 
            && this.MessageTopic == other.MessageTopic 
            && this.MessageType == other.MessageType) return true;
        return false;
    }
    public override int GetHashCode()
    {
        int total = 0;
        foreach (var item in this.Name)
            total += 11 * (int)item;
        total += 11 * this.MessageType.GetHashCode();
        total += 11 * this.MessageTopic.GetHashCode();
        return total;
    }
}