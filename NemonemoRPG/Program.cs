using System;
using System.Collections.Generic;
using System.Threading;

class Item
{
    public string Name { get; }
    public string Description { get; }
    public int Attack { get; }
    public int Defense { get; }
    public bool Equipped { get; set; }

    public Item(string name, string description, int attack, int defense)
    {
        Name = name;
        Description = description;
        Attack = attack;
        Defense = defense;
        Equipped = false;
    }

    public string GetDisplay()
    {
        string prefix = Equipped ? "[E]" : "   ";
        string stats = "";

        if (Attack > 0) stats += $"공격력 +{Attack}";
        if (Defense > 0) stats += (stats != "" ? " | " : "") + $"방어력 +{Defense}";

        return $"{prefix}{Name,-12} | {stats,-12} | {Description}";
    }
}

class ShopItem
{
    public string Name { get; }
    public string Description { get; }
    public int Attack { get; }
    public int Defense { get; }
    public int Price { get; }
    public bool IsPurchased { get; set; }

    public ShopItem(string name, string description, int attack, int defense, int price)
    {
        Name = name;
        Description = description;
        Attack = attack;
        Defense = defense;
        Price = price;
        IsPurchased = false;
    }

    public string GetDisplay()
    {
        string stats = "";
        if (Attack > 0) stats += $"공격력 +{Attack}";
        if (Defense > 0) stats += (stats != "" ? " | " : "") + $"방어력 +{Defense}";

        string priceStr = IsPurchased ? "구매완료" : $"{Price} G";

        return $"- {Name,-12} | {stats,-10} | {Description,-35} | {priceStr,10}";
    }
}

class Program
{
    static string playerName = "네모기사";
    static int level = 1;
    static int hp = 100;
    static int baseAtk = 10;
    static int baseDef = 5;
    static int gold = 800;

    static List<Item> inventory = new List<Item>();
    static List<ShopItem> shopItems = new List<ShopItem>
    {
        new ShopItem("네모셔츠", "완벽하게 각진 셔츠입니다.", 0, 5, 1000),
        new ShopItem("각진갑옷", "입으면 왠지 광산에 가고 싶어집니다.", 0, 9, 2000) { IsPurchased = true },
        new ShopItem("절대자의 로브", "착용자의 심신을 빈틈없이 채워줍니다", 0, 15, 3500),
        new ShopItem("동그라미", "네모마을에서는 배척받는 흉물.", 2, 0, 600),
        new ShopItem("각진도끼", "들고 있으면 왠지 나무를 캐고 싶어집니다.", 5, 0, 1500),
        new ShopItem("절대자의 큐브", "이 무한한 힘에 사각지대는 없습니다.", 7, 0, 3000) { IsPurchased = true }
    };

    static void Main()
    {
        inventory.Add(new Item("네모셔츠", "완벽하게 각진 셔츠입니다.", 0, 5) { Equipped = true });
        inventory.Add(new Item("사각자", "이제 대세는 사각자입니다", 7, 0) { Equipped = true });
        inventory.Add(new Item("동그라미", "네모마을에서는 배척받는 흉물.", 2, 0));

        while (true)
        {
            SlowPrint("\n🎮 네모네모 마을에 오신 것을 환영합니다!");
            SlowPrint("당신은 끔찍한 악의 소굴 '네모네모 던전'에 들어가 보물을 찾아내야합니다.\n");
            SlowPrint("이곳에서 던전에 들어가기 전 준비를 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("0. 게임 종료");
            Console.Write("\n원하시는 행동을 입력해주세요 >> ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowStatus();
                    break;
                case "2":
                    ShowInventory();
                    break;
                case "3":
                    ShowShop();
                    break;
                case "0":
                    SlowPrint("게임을 종료합니다.");
                    return;
                default:
                    SlowPrint("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }

    static void ShowStatus()
    {
        while (true)
        {
            int equipAtk = baseAtk;
            int equipDef = baseDef;

            foreach (var item in inventory)
            {
                if (item.Equipped)
                {
                    equipAtk += item.Attack;
                    equipDef += item.Defense;
                }
            }

            SlowPrint("\n📜 [플레이어 상태]");
            Console.WriteLine($"이름: {playerName}");
            Console.WriteLine($"레벨: {level}");
            Console.WriteLine($"체력: {hp}");
            Console.WriteLine($"공격력: {equipAtk}");
            Console.WriteLine($"방어력: {equipDef}");
            Console.WriteLine($"보유 골드: {gold} G");

            Console.WriteLine("\n0. 메인 메뉴로 돌아가기");
            Console.Write("\n입력 >> ");
            string? input = Console.ReadLine();

            if (input == "0")
                break;
            else
                SlowPrint("잘못된 입력입니다. 메인 메뉴로 돌아가려면 0을 입력하세요.");
        }
    }

    static void ShowInventory()
    {
        while (true)
        {
            SlowPrint("\n🎒 인벤토리");
            SlowPrint("보유 중인 아이템을 관리할 수 있습니다.\n");

            if (inventory.Count == 0)
            {
                Console.WriteLine("보유 중인 아이템이 없습니다.\n");
                Console.WriteLine("0. 나가기");
            }
            else
            {
                Console.WriteLine("[아이템 목록]");
                foreach (var item in inventory)
                {
                    Console.WriteLine("- " + item.GetDisplay());
                }

                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
            }

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string? input = Console.ReadLine();

            if (input == "0")
                break;
            else if (input == "1" && inventory.Count > 0)
                ManageEquip();
            else
                SlowPrint("잘못된 입력입니다. 다시 입력해주세요.");
        }
    }

    static void ManageEquip()
    {
        Console.WriteLine("\n🔧 장착할 아이템 번호를 선택하세요:");
        for (int i = 0; i < inventory.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {inventory[i].GetDisplay()}");
        }

        Console.Write("\n선택 >> ");
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice >= 1 && choice <= inventory.Count)
            {
                foreach (var item in inventory)
                    item.Equipped = false;

                inventory[choice - 1].Equipped = true;
                SlowPrint($"{inventory[choice - 1].Name}을(를) 장착했습니다!");
            }
            else
            {
                SlowPrint("유효하지 않은 번호입니다.");
            }
        }
        else
        {
            SlowPrint("숫자로 입력해주세요.");
        }
    }

    static void ShowShop()
{
    while (true)
    {
        SlowPrint("\n🏪 상점 - 아이템 구매");
        SlowPrint("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine($"[보유 골드]\n{gold} G\n");
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < shopItems.Count; i++)
        {
            Console.WriteLine($"- {i + 1} {shopItems[i].GetDisplay()}");
        }

        Console.WriteLine("\n0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
        string? input = Console.ReadLine();

        if (input == "0")
            break;
        else
            BuyItem(input);
    }
}

   static void BuyItem(string input)
{
    if (!int.TryParse(input, out int index) || index < 1 || index > shopItems.Count)
    {
        SlowPrint("잘못된 입력입니다.");
        return;
    }

    var item = shopItems[index - 1];

    if (item.IsPurchased)
    {
        SlowPrint("이미 구매한 아이템입니다.");
    }
    else if (gold < item.Price)
    {
        SlowPrint("Gold가 부족합니다.");
    }
    else
    {
        inventory.Add(new Item(item.Name, item.Description, item.Attack, item.Defense));
        item.IsPurchased = true;
        gold -= item.Price;
        SlowPrint("구매를 완료했습니다.");
    }
}


    static void SlowPrint(string text, int delay = 30)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }
}
