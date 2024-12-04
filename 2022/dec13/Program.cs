using System.Text.Json;

Console.Clear();
var input = File.ReadAllLines("input.txt");

var result = 0;
var index = 1;
List<MixedObject> allSignals = new List<MixedObject>();
for(int i = 0; i < input.Length; i += 3){
    var pair = input[i..(i+2)];
    var left = new MixedObject(JsonSerializer.Deserialize<JsonElement>(pair[0]));
    var right = new MixedObject(JsonSerializer.Deserialize<JsonElement>(pair[1]));
    allSignals.Add(left);
    allSignals.Add(right);

    var same = left.CompareTo(right);

    if(same == -1){
        result += index;
    }
    index++;
    

}
MixedObject d1 = new MixedObject(JsonSerializer.Deserialize<JsonElement>("[[2]]"));
MixedObject d2 = new MixedObject(JsonSerializer.Deserialize<JsonElement>("[[6]]"));
allSignals.Add(d1);
allSignals.Add(d2);

Console.WriteLine(result);


allSignals.Sort();
var indexOf1 = allSignals.IndexOf(d1)+1;
var indexOf2 = allSignals.IndexOf(d2)+1;

Console.WriteLine(indexOf1 + " * " + indexOf2);
Console.WriteLine(indexOf1*indexOf2);

class MixedObject : IComparable<MixedObject>{

    public List<MixedObject>? List { get; set; }
    public int? Integer { get; set; }
    public MixedObject(JsonElement input){
        if(input.ValueKind == JsonValueKind.Array){
            List = new List<MixedObject>();
            foreach(var e in input.EnumerateArray()){
                List.Add(new MixedObject(e));
            }
        }else{
            Integer = input.GetInt32();
        }
    }
    public MixedObject(){

    }

    public int CompareTo(MixedObject? other){
        if(this.Integer != null && other.Integer != null){
            if(this.Integer == other.Integer){
                return 0;
            }
            if(this.Integer < other.Integer){
                return -1;
            }
            return 1;
        }
        List<MixedObject>  thisAsList = this.MakeToList();
        List<MixedObject>  otherAsList =other.MakeToList();
        int i = 0;
        foreach(MixedObject m in thisAsList! ){
            if(i == otherAsList.Count){
                return 1;
            }
            var res = m.CompareTo(otherAsList[i]);
            if(res != 0){
                return res;
            }
            i++;
        }
        if(thisAsList.Count < otherAsList.Count){
            return -1;
        }
        return 0;
    }

    public List<MixedObject> MakeToList(){
        if(List != null){
            return List;
        }
        return new List<MixedObject>(){new MixedObject(){
            Integer = this.Integer
        }};
        // Integer = null;
    }

    public override string ToString()
    {   
        if(Integer != null){
            return Integer.ToString();
        }
        return "[" +string.Join(",", List) + "]";
    }
}

enum Result{
    True,
    Continue,
    False
}


// bool Compare(JsonElement left, JsonElement right){
    


//     bool leftIsArray = left.ValueKind == JsonValueKind.Array;
//     bool rightIsArray = left.ValueKind == JsonValueKind.Array;

//     if(left.ValueKind != right.ValueKind){
//         left = MakeArray(left);
//     }

//     Console.WriteLine(left.ValueKind);
//     Console.WriteLine(right.ValueKind);
//     return false;
// }



// class MixedObject<T> where T : int, List<MixedObject>{
//     public int MyProperty { get; set; }

// }

// class MixedList : List<MixedObject>{

// }