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
- I'm the mayor and I did nothing wrong
+ {player_has(evidence) && player_has(recorder)} "Check out this evidence" \(secretly record what he says next\)
    -> mayor_record
+ {player_has(evidence) && not player_has(recorder)} "Check out this evidence"
    -> mayor_confess
+ Okay
    -> END
    
== mayor_confess ==
- Yeah, that's the evidence that I did something wrong! But no one will believe you without a recording!
+ Okay
    -> END
    
== mayor_record ==
- Yeah, that's the evidence I did something wrong! Whaaaat, you recorded me? I guess you win!
+ Okay
    -> END
    
== mayor_daughter_start ==
- I'm the mayor's daughter
+ Okay
    -> END
    
== mathProf_start ==
- I'm the Math Professor. I need my lesson plan so I can teach my students how to gamble!
+ {player_has(lessonPlan)} "Oh you have my lesson plan! Thank you!"
    -> mathProf_trade
+ Okay
    -> END
    
== mathProf_trade ==
- \(You sit in on the Professor's lecture. You feel like you are good at cards now!\) # take_lessonPlan
+ Okay
    -> END

== architect_start ==
- I'm the architect, wanna learn to see like an architect does?
+ {not architect_teach} "Teach me"
    -> architect_teach
+ Okay
    -> END
    
== architect_teach ==
- Now you'll be able to see walls that can be destroyed with an Explosive  # grant_architect
+ Okay
    -> END
    
== acrobat_start ==
- I'm the acrobat. If you bring me my clown nose from the office I'll teach you to zipline!
+ {player_has(clownNose)} "Here's your nose"
    -> acrobat_trade
+ Okay
    -> END

== acrobat_trade ==
- Yes! Now I can do my act. Here is the secret to ziplining!  # take_clownNose  # grant_zipline
+ Okay
    -> END
    
== bootlegger_start ==
- I'm the bootlegger! Wanna learn bootlegger vision?
+ {not bootlegger_teach} "Teach me!"
    -> bootlegger_teach
+ Okay
    -> END
    
== bootlegger_teach ==
- Bootleggers can see hidden crime doors! Now you can too!  # grant_bootlegger
+ "Thanks!"
    -> END
    
== climber_start ==
- I'm the climber. I'll teach you to climb for a dollar
+ {not climber_trade && player_has(dollar)} "Sure, teach me to climb"
    -> climber_trade
+ Okay
    -> END
    
== climber_trade ==
- Tah dah! Now you know how to climb  # take_dollar # grant_climb
+ Okay
    -> END
    
== luggageman_start ==
- I'm the guy that cleans luggage. Do you have a ticket and a dollar?
+ {player_has(ticket) && player_has(dollar)} "Yes I do!"
    -> luggageman_trade
+ Okay
    -> END
    
== luggageman_trade ==
- Here's your extra big suitcase sir!  # take_dollar # take_ticket # upgrade_inventory
+ \(Forget the CEO, you decide to keep it\)
    -> END
    
== visionman_start ==
- I'm some kind of opthamologist or something. I can teach you to see in the dark, but I need a lens.
+ {player_has(lens)} "I've got your lens"
    -> visionman_trade
+ Okay
    -> END
    
== visionman_trade ==
- Bam! You can see in the dark now!  # take_lens # grant_darkVision
+ Okay
    -> END
    
== audioEngineer_start ==
- I'm the radio scientist. I need wire to build a portable recording device
+ {player_has(wire)} "Here's the wire you wanted"
    -> audioEngineer_trade
+ Okay
    -> END
    
== audioEngineer_trade ==
- Check out this portable recording device! # take_wire
+ Okay
    -> audioEngineer_give
    
== audioEngineer_give ==
- \(receive item\) # give_recorder
    -> END

== wireman_start ==
- I'm the man that sells wire. Want some for a dollar?
+ {dialogue_has(wire) && player_has(dollar)} Here you go sir
    -> wireman_trade   
+ Okay
    -> END
    
== wireman_trade ==
- Nice doing business with you # take_dollar
+ Okay
    -> wireman_give
    
== wireman_give ==
- \(receive item\) # give_wire
    -> END
    
== ceo_start ==
- I'm the rich executive.
+ {dialogue_has(ticket)} Anything I can do for you?
    -> ceo_quest
+ Okay
    -> END
    
== ceo_quest ==
- Yeah, pick up my luggage for me, would you? You'll need to use this ticket to get it from the cleaners.
+ Okay
    -> ceo_give
    
== ceo_give ==
- \(receive item\) # give_ticket
    -> END
    
== mobBoss_start ==
- I'm the mob boss ayyyy. One of my boys hid an innocuous parcel in the skyscraper, would you fetch it for me?
+ {player_has(package)} Here's the innocuous parcel you wanted
    -> mobBoss_trade
+ Okay
    -> END
    
== mobBoss_trade ==
- Thanks, lemme give you a special suitcase lining that can trick metal detectors!  # grant_antiMetalDetector  # take_package
+ Okay
    -> END
    
== gambler_start ==
- I'm the gambler, you can play cards for a dollar! If you win, I'll teach you the secret knock for gangster doors!
+ {not gambler_lose && not mathProf_trade && player_has(dollar)} Play cards badly
    -> gambler_lose
+ {gambler_lose && not mathProf_trade && player_has(dollar)} Play cards badly
    -> gambler_lose_again
+ {mathProf_trade && player_has(dollar)} Play cards well
    -> gambler_win
+ Okay
    -> END

== gambler_lose ==
- \(You lose your dollar playing cards. Maybe someone can teach you to play better\)  # take_dollar
+ Okay
    -> END
    
== gambler_lose_again ==
- \(You decide not to risk another dollar until you learn how to play cards better\)
+ Okay
    -> END

== gambler_win ==
- Jeez, you won! Okay, the secret knock is five knocks in a row!  # take_dollar
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