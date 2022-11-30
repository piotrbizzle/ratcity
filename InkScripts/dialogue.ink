// inventory stuff
LIST all_items = DummyItem1, DummyItem2, DummyItem3
VAR player_inventory = ()

== function inventory_has(item) ==
~ return player_inventory has item

== gross_guy ==
- "You made it to the end! Great job!" # give_test
+ "Thank you"
    -> thanks
+ "You are too gross."
    -> gross
+ {inventory_has(DummyItem1)} Secret Option
    -> END
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