// inventory stuff
LIST all_items = coin, dollar, rope, explosive, lessonPlan, evidence, clownNose, lens, wire, package, ticket, recorder
VAR player_inventory = ()
VAR dialogue_inventory = ()

== function player_has(item) ==
~ return player_inventory has item

== function dialogue_has(item) ==
~ return dialogue_inventory has item

// character dialogues
== mayor_start ==
- I'm the mayor
+ Okay
    -> END
    
== mayor_daughter_start ==
- I'm the mayor's daughter
+ Okay
    -> END
    
== mathProf_start ==
- I'm the Math Professor
+ Okay
    -> END

== architect_start ==
- I'm the architect
+ Okay
    -> END
    
== acrobat_start ==
- I'm the acrobat
+ Okay
    -> END
    
== bootlegger_start ==
- I'm the bootlegger
+ Okay
    -> END
    
== climber_start ==
- I'm the climber
+ Okay
    -> END
    
== luggageman_start ==
- I'm the guy that cleans luggage
+ Okay
    -> END
    
== visionman_start ==
- I'm some kind of opthamologist or something
+ Okay
    -> END
    
== audioEngineer_start ==
- I'm the radio scientist
+ Okay
    -> END
    
== wireman_start ==
- I'm the man that sells wire
+ Okay
    -> END
    
== ceo_start ==
- I'm the rich executive 
+ Okay
    -> END
    
== mobBoss_start ==
- I'm the mob boss ayyyy
+ Okay
    -> END
    
== gambler_start ==
- I'm the gambler yee ha
+ Okay
    -> END
    

// environment descriptions
== tutorial_1 ==
- tutorial_1
+ okay
    -> END

// item descriptions
== coin_desc ==
- A lucky coin that gets you in trouble... just my luck
+ Okay
    -> END
    
== dollar_desc ==
- A dollar. Can pay for stuff
+ Okay
    -> END
    
== rope_desc ==
- A rope. Can extend ziplines
+ Okay
    -> END

== explosive_desc ==
- An explosive. Can break weak walls
+ Okay
    -> END
    
== lessonPlan_desc ==
- A lesson plan. Seems to be about probability
+ Okay
    -> END
    
== evidence_desc ==
- The evidence. The mayor will confess if he sees this
+ Okay
    -> END

== clownNose_desc ==
- A bright red clown nose. The performer in the park needs this
+ Okay
    -> END
    
== lens_desc ==
- A special lens. Maybe someone can use this
+ Okay
    -> END
    
== wire_desc ==
- A spool of copper wire. Good for radios
+ Okay
    -> END
    
== package_desc ==
- A suspicious parcel. Could be anything in there
+ Okay
    -> END
    
== ticket_desc ==
- A ticket from Luey's Luggage Cleaners. The luggage should be ready today
+ Okay
    -> END
    
== recorder_desc ==
- A portable recording device, depending on your definition of "portable"
+ Okay
    -> END