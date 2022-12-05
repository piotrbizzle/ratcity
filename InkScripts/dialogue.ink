// inventory stuff
LIST all_items = luckyCoin
VAR player_inventory = ()
VAR dialogue_inventory = ()

== function player_has(item) ==
~ return player_inventory has item

== function dialogue_has(item) ==
~ return dialogue_inventory has item

// character dialogues


// environment descriptions
== tutorial_1 ==
- tutorial_1
+ okay
    -> END
 
== letter ==
- letter
+ okay
    -> END
    
== out_of_order ==
- out_of_order
+ okay
    -> END

// item descriptions
== lucky_coin ==
- a lucky coin that gets you in trouble... just my luck
+ okay
    -> END

