// inventory stuff
LIST all_items = DummyItem1, DummyItem2, DummyItem3
VAR player_inventory = ()
VAR dialogue_inventory = ()

== function player_has(item) ==
~ return player_inventory has item

== function dialogue_has(item) ==
~ return dialogue_inventory has item

== gross_guy ==
- "You made it to the end! Great job!" # give_test
+ "Thank you"
    -> thanks
+ "You are too gross."
    -> gross
+ {player_has(DummyItem1)} "Have this dummy item!"
    -> steal
+ {dialogue_has(DummyItem1)} "Can i have my dummy item back?"
    -> give_back
+ \(Walk away\)
    -> END
    
== thanks ==
- "No Problem!"
+ \(Leave Him\)
    -> END
    
== gross ==
- "How cruel!"
+ \(Leave Him\)
    -> END
    
== steal ==
"YOINK!" # take_DummyItem1
+ \(Leave Him\)
    -> END

== give_back ==
"Fine here" # give_DummyItem1
+ \(Leave Him\)
    -> END