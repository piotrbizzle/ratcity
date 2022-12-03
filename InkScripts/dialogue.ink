// inventory stuff
LIST all_items = DummyItem1, DummyItem2, DummyItem3
VAR player_inventory = ()
VAR dialogue_inventory = ()

== function player_has(item) ==
~ return player_inventory has item

== function dialogue_has(item) ==
~ return dialogue_inventory has item

== other_gross_guy ==
- "I may look like a Gross Guy but I know some good tips!"
+ "What is the secret knock?"
  -> secret_knock
+ "How can I see in the dark?"
  -> see_in_the_dark
+ \(Walk away\)
    -> END
  
== secret_knock ==
- "The secret knock is: knock, knock... knock!"
+ "Thanks, pal!"
    -> END

== see_in_the_dark ==
- "I can teach you to see in the dark, but you better bring me a Pink Item!"
+ {player_has(DummyItem3)} "I've got it right here, boss!"
    -> dark_vision
+ "Let me seeeee..."
  -> END

== dark_vision ==
- \(The Gross Guy licks each of your eyeballs. You feel like you can see in the dark now!\) # take_DummyItem3 # grant_darkVision
+ "Uhhhhh thanks I think"
  -> END

== gross_guy ==
- "I'll give you a Pink Item in exchange for a Green one, but I reaaaally want a Blue Item!"
+ {player_has(DummyItem2) && dialogue_has(DummyItem3)} "Let's trade!"
    -> trade
+ {player_has(DummyItem3) && dialogue_has(DummyItem2)} "Let's trade back"
    -> trade_back
+ {dialogue_has(DummyItem2)} \(Steal your Green Item back\)
    -> steal
+ {player_has(DummyItem1)} "I have that Blue Item you wanted!"
    -> win
+ \(Walk away\)
    -> END
    
== trade ==
- "Nice doing business with you!" # take_DummyItem2 # give_DummyItem3
+ \(Leave Him\)
    -> END
    
== trade_back ==
"Okay, let's trade back!" # take_DummyItem3 # give_DummyItem2
+ \(Leave Him\)
    -> END
    
== steal ==
- \(He doesn't suspect a thing\) # give_DummyItem2
+ \(Leave Him\)
    -> END

== win ==
"That's perfect! You're the winner!" # take_DummyItem1
+ \(Leave Him\)
    -> END
    
== gross_guy_3 == 
- "Wanna learn to zipline?"
+ "Sure"
    -> zipline
+ \(Leave Him\)
    -> END
    
== zipline ==
- "Just jump up to that red dot while scurrying!" # grant_zipline
+ \(You feel like you can zipline now\)
    -> END

== description ==
- "It's like... a weird angry oval?"
+ \(Look away\)
    -> END