// inventory stuff
LIST all_items = coin, dollar, rope, explosive, lessonPlan, evidence, clownNose, lens, wire, package, ticket, recorder, files
VAR player_inventory = ()
VAR dialogue_inventory = ()

== function player_has(item) ==
~ return player_inventory has item

== function dialogue_has(item) ==
~ return dialogue_inventory has item

// character dialogues
== longJump_start ==
- {not longJump_cloudy: Good afternoon! What a beautiful day to stroll through The Park! | Did you try the Long Jump yet, mister? Don't forget to hold the S key on the run-up!}
+ {not longJump_cloudy} "It's a little cloudy..."
  -> longJump_cloudy
+ {longJump_cloudy} "Yeah, yeah."
  -> END

== longJump_cloudy ==
- Cloudy? I don't think so. It's probably just the shade from those platforms above us.
+ "Hm. Could be."
  -> longJump_platforms

== longJump_platforms ==
- Talking about platforms, those ones look like a perfect place to try a Long Jump!
+ "Excuse me?"
  -> longJump_howTo

== longJump_howTo ==
- Sure! Just hold the S key to Scurry, get a running start, and with the right timing I bet you could clear the gap!
+ "Maybe I'll give it a try."
  -> END
  

== elevator_start ==
- This elevator may be fancy but it's so slow! I've been waiting for it here forever! Ridiculous!
+ "Uh huh."
  -> END


== cop_start ==
- New security measures in place ahead of the election, sir. You can't bring anything Metal into City Hall.
+ "How can you tell if I've got anything Metal?"
  -> cop_metal_detector
+ "Nothing Metal, got it."
  -> END

== cop_metal_detector ==
- PelliCorp donated a Metal Detector to the city! If you're carrying any Metal, you won't be able to get through the entryway.
+ "Supposing, hypothetically, I do have something made of Metal on me, what should I do?"
  -> cop_hypothetical

== cop_hypothetical ==
- 'Hypothetically', if you leave it out here, I'd be happy to keep an eye on it for you, sir.
+ "How do I do that again?"
- Open your inventory with the I key, move any Metal items out of the bounds of your briefcase, then close your inventory.
+ "Sublime."
  -> END

== gambler_start ==
- Hey mister! Wanna play cards? I can't stop playing Gin Rummy! I'll play for anything!
+ {not gambler_information} "How about playing for information?"
  -> gambler_information
+ {gambler_information and not gambler_secretKnock} "I'm game."
  -> gambler_play
+ {gambler_secretKnock} "Say, what's the Secret Knock for Crime Doors again?"
  -> gambler_secretKnock
+ "Maybe another time."
  -> END

== gambler_information ==
- Information!? Well, I DO know a juicy tip. How about this: you put down a dollar to play, and if you win, I'll tell you what I know!
+ "Wait so you get the dollar no matter what?"
  -> gambler_dollar

== gambler_dollar ==
- Yes sir yes sir, that's the game.
+ {player_has(dollar)} "Alright, I'll bite."
  -> gambler_play
+ "Maybe another time."
  -> END

== gambler_play ==
- The game is Gin Rummy! Ante's a dollar!
+ {not mathProf_trade and not gambler_badAtCards and player_has(dollar)} \(Play cards badly\)
  -> gambler_badAtCards
+ {mathProf_trade and player_has(dollar)} \(Apply your knowledge of statistics and play cards well\)
  -> gambler_goodAtCards
+ "On second thought, I'll come back later."
  -> END

== gambler_badAtCards ==
- I win! God DAMN do I love Gin Rummy!  # take_dollar
+ "Congratulations." \(Maybe you should come back when you're better at playing cards\)
  -> END

== gambler_goodAtCards ==
- I lose??? God DAMN do I hate Gin Rummy! # take_dollar
+ "Fair's fair, let's have that spicy information you promised me."
  -> gambler_win

== gambler_win ==
- Alright alright. You know those metal doors with the Black Mask Gang's symbol on them? Those are Crime Doors.
+ "Crime Doors?"
  -> gambler_crimeDoors

== gambler_crimeDoors ==
- Yeah! Criminal types use them to hide stuff from the cops. They'll only open if you do the Secret Knock.
+ "So what's the Secret Knock?"
  -> gambler_secretKnock

== gambler_secretKnock ==
- \(He leans in and whispers\) The Secret Knock is... Five knocks in a row, then wait!
+ "Five knocks in a row, got it."
  -> END

== wireman_start ==
- Welcome to Buy Yer Wire! How can I help you, sir?
+ "You just sell wire here?"
  -> wireman_pitch
+ {player_has(dollar) and dialogue_has(wire) and audioEngineer_saveThePark} "A spool of Copper Wire, if you've got it."
  -> wireman_trade  
+ "I'm just browsing."
  -> END

== wireman_pitch ==
- JUST wire!? My dear sir, at Buy Yer Wire we purvey only the \~finest\~ and \~highest quality\~ and \~most luxurious\~ wire around.
+ "Sure, but /only/ wire?"
  -> wireman_onlyWire

== wireman_onlyWire ==
- As a specialty shop, Buy Yer Wire is able to give our customers that special expertise they depend on for all their wire purchases.
+ "Ten four."
  -> END

== wireman_trade ==
- It's a pleasure to serve such a discerning customer! One spool of Copper Wire coming right up, sir! # take_dollar
+ "Great, thanks."
  -> wireman_give

== wireman_give ==
- YOU GOT WIRE # give_wire
+ "Okay"
    -> END


== luggageman_start ==
- Oh. Hello sir. Did you come in to ask for directions to a less fancy store? Our fine luggage is /premium/ and clients usually order months in advance.
+ {player_has(ticket)} "Actually, I have a ticket to pick up a new briefcase"
  -> luggageman_ticket
+ "You know what, I was just on my way out"
  -> END

== luggageman_ticket ==
- I see, you must be picking up on a more wealthy person's behalf. You'll need to put down a dollar for the pickup fee, will that be a problem?
+ {player_has(dollar)} "No problem at all."
  -> luggageman_trade
+ "Pickup fee? You hold on to that briefcase for now. I'll be back later"
  -> END

== luggageman_trade ==
- Excellent. Here's the case, sir. Be sure it arrives safely. # take_ticket # take_dollar # upgrade_inventory
+ "Wow, this briefase is huge! I'll just move my stuff into it." 
  -> END


== bouncer_start ==
- Nobody but nobody gets in to see Boss Chaff until the election's over. Scram.
+ "You got it. Great tux by the way."
  -> END


== acrobat_start ==
- {not acrobat_trade: Hey mister, I recognize you from my building! I'm supposed to do my big Ziplining Clown Show today, but I forgot my clown nose. | Thanks again for your help, neighbor! Don't forget to hold the S key on Hook Symbols to Zipline!}
+ {not acrobat_trade} "Ziplining Clown Show? Sounds dangerous."
  -> acrobat_clownShow
+ {player_has(clownNose)} "I've got your nose."
  -> acrobat_trade
+ {acrobat_trade} "Got it."
  -> END
+ {not acrobat_trade} I'll see what I can do
  -> END

== acrobat_clownShow ==
- It is! Plus, getting the permits was a real nightmare, so it has to be today! Could you stop by our building and grab my nose please?
+ "Neighbors helping neighbors, why not?"
  -> END

== acrobat_trade ==
- You're a lifesaver! Here, let me teach you to Zipline. Just hold down the S key by one of those Hook Symbols and you'll glide like a bird! # take_clownNose # grant_zipline # modify_sprites
+ "That's all there is to it?"
  -> acrobat_zipline

== acrobat_zipline ==
- Sometimes a Hook Symbol won't have a line coming out of it. In that case, you can Interact with it while carrying a Rope to activate the Zipline!
+ "Ropes activate Ziplines. Easy enough."
  -> END

== mathProf_start ==
- {not mathProf_trade: Oh jeepers jeepers, what am I going to do? | Statistically speaking, you're one in a million! Just kidding, you're actually one in a much bigger number.}
+ {mathProf_trade} "Heh."
  -> END
+ {not mathProf_trade} "What's the problem?"
  -> mathProf_problem
+ {mathProf_problem and player_has(lessonPlan)} "You needed a lesson plan?"
    -> mathProf_trade
+ {not mathProf_trade} "I'll be back in a bit"
    -> END

== mathProf_problem ==
- I'm supposed to give a lecture on Statistics, but I've misplaced my black notebook with my lesson plan in it!
+ "Any idea where you left it?"
  -> mathProf_leftIt
+ {player_has(lessonPlan)} "Is this it here?"
  -> mathProf_trade
  
== mathProf_leftIt ==
- I bet it's over in PelliCorp Tower. I consult there sometimes. The money's too good not to!
+ "I'll take a look, see what turns up."
  -> END
  
== mathProf_trade ==
- Bless you for a saint! I don't have any money to pay you, but why don't you sit in on my lecture today?  # take_lessonPlan
+ "I thought you said the PelliCorp money was good?"
  -> mathProf_money
  
== mathProf_money ==
- Ahhhh, you don't need money, though. Trust me, the lecture is worth it's weight in gold!
+ \(Listen to the lecture and try not to think about how much it weighs.\)
    -> mathProf_lecture
    
== mathProf_lecture ==
- \(This Statistics stuff seems pretty useful. You feel like you'll do better in games of chance going forward.\)
+ "Thanks for the lesson, Teach"
    -> END


== mayor_start ==
- {not mayor_letter: Greetings, citizen! Can I count on your vote next Tuesday? | Get out of my office, rat.}
+ {not mayor_things} "Howdy, Mr. Mayor. How's things?"
    -> mayor_things
+ {mayorDaughter_problem} "I spoke to your daughter about a letter she found in your office."
    -> mayor_letter
+ {mayor_letter and player_has(evidence) and not player_has(recorder)} "I found some really interesting papers in a hidden room in the Mayor's Mansion."
    -> mayor_evidence
+ {mayor_letter and player_has(evidence) and player_has(recorder)} \(Secretly record the Mayor.\)
    -> mayor_record
+ "You seem busy."
    -> END

== mayor_things ==
- It's an exciting time for all of us!
+ "Aren't you nervous about the election?"
- In just a few days, my consituents will vote to keep me in office, and I'll be privileged to help this city thrive for another term!
+ "You seem pretty confident."
- I trust our voters to make the right decision.
+ "Great."
    -> END
    
== mayor_letter ==
- ...Are you threatening me?
+ "Just keeping you in the loop."
- Let me keep YOU in the loop. You're that PI who solved that big Roquefort Diamond case a few years back, right?
+ "You've heard of me."
- I also heard the only cases you've solved since then were full of cheap liquor.
+ "I'm pretty sure the word 'case' specifically implies beer."
- I'll trust you on the nomenclature. Do me a favor and keep your losing streak going through election day. I know what I'm doing.
+ "Your daughter asked me to help."
- Here's how you can help: go back to your office, sit your rear-end down, and don't do a thing until the polls close.
+ "Suppose I have a personal curiosity about this case?"
- You don't. Do nothing, collect whatever money my daughter offered you, then go back to being a former flash-in-the-pan news story.
+ "That's some tough love, Mr. Mayor."
    -> END
    
== mayor_record ==
- \(You switch on the enormous Portable Recording Device in your briefcase.\)
+ "I found some really interesting papers in a hidden room in the Mayor's Mansion."
    -> mayor_evidence
    
== mayor_evidence ==
- That old Treaty from the City Records? Who cares?
+ "Who cares?"
- Sure, I'm going to demolish The Park and build our nation's first Mall,
+ "I'm still not quite sure what that is."
- ..sure, I took campaign donations from PelliCorp to do so,
+ "That seems pretty serious, but maybe not illegal."
- ...and SURE, he promised to 'encourage' all of his employees to vote for me,
+ "THAT'S got to be illegal."
- ...but guess what? All you have is some old papers. Papers that are missing from the City Records, because YOU have them.
+ "Shoot."
- So, it'll be my word versus yours. This city's beloved Mayor versus an attention-seeking bum, desperate to be relevant again.
+ "But you need the Treaty out of the way to build the Mall, right?"
- It'll take a little longer, but there's always a way around everything. Pellicle will understand the delay.
+ "So me having these papers means..."
- Nothing at all. Thanks for stopping by, Detective.
+ "Shucks"
    -> END
    
== mayorDaughter_start ==
- {not mayorDaughter_problem:  Mister-- er, Detective Sweetwhey, thanks for getting here so quickly. I couldn't tell you all the details in my letter, it's not safe! | Have you found the blackmailer yet?}
+ {not mayorDaughter_problem} "It's Corporal Sweetwhey if you want to get technical. What's the problem?"
    -> mayorDaughter_problem
+ {mayorDaughter_problem} "Can you run me through everything again?"
    -> mayorDaughter_problem
+ {mayorDaughter_problem} "It's a work in progress."
    -> END
    
== mayorDaughter_problem ==
- Someone is threatening to release damaging information about my father just a few days before the election!
+ "I heard the mayoral race is supposed to be awfully close."
- It is! The pollsters say it could go either way!
+ "If someone wants him to lose, why don't they just release the info?"
- Well, it's blackmail of course! I found a letter in my father's study demanding ten thousand dollars to keep quiet.
+ "That's some real walking-around money. You gonna pay?"
- Would I hire you if I was? I need you to find whoever's doing this and keep them quiet at least until the election's done.
+ "So you're spending a little money to save a lot. Pretty smart"
- I'm feeling less smart about it by the second. Can you do it?
+ "I can poke around a little, with the right motivation."
- Do you say that to all the rich young women you meet?
+ "Heh. Before I can quote you a price, what do we know so far?
- I've got three suspects in mind already.
+ "Your old man's got a lot of enemies"
- That's the cost of public office, I suppose.
+ "Fair enough. Who's got it out for him?"
- The first suspect is his opponent, Gus Crumb. If he can't win the election, he might be trying to make some money off it instead.
+ "Quite a consolation prize. But you said the race was close."
- Crumb's got his own skeletons in the closet. Maybe he's worried he can't win.
+ "Maybe. Who else could it be?"
- The next suspect is that awful reporter, Rebecca Bran. She's been writing hit pieces on my father his whole career!
+ "She can't be that bad..."
- Believe me, she is. A nasty gossip like that is definitely not above blackmail.
+ "And your third lead?"
- The last suspect is Boss Chaff. That brute has his hands in everything criminal that happens around here.
+ "Not very specific, but also not a bad guess."
- Now you know the score, can you help me or not?
+ "It'll be five hundred up front, five hundred when the job's done"
- That's awfully steep!
+ "Think of it as a 90% savings."
- I'm not paying if you mess this up. How about twelve hundred total, payable after the election?
+ "You seem trustworthy. Throw in expenses and you've got a deal."
- Splendid -- and Detective Sweetwhey, however you do it, please be discreet.
+ "That's extra."
    -> END


== architect_start ==
- {not architect_teach: Whoa hey! Office hours are cancelled! I'm working hard on this research grant here. | Sweetwhey! How is my star pupil doing?"}
+ {not architect_teach} "It looks like a wall..."
    -> architect_teach
+ {architect_teach} "Just fine, Professor. Can you walk me through how Explosive work again?"
    -> architect_explosives
+ {player_has(files)} "Can you take a look at these blueprints for me?
    -> architect_files
    
== architect_teach == 
- To YOU maybe, but to an Architect, every wall is a door -- with the right Explosives!
+ "Excuse me?"
- I'm researching demolition techniques for grass fields, floating platforms. massive fountains, things like that.
+ "Who would pay you to research that?"
- PelliCorp has graciously extended a grant for my work. The Asiago Institute for Higher Learning is grateful to our corporate sponsors.
+ "I dunno, seems pretty straightforward to blow stuff up."
- Oh on the contrary! You need an Eye for Architecture to spot the best areas to place Explosives.
+ "Is that something an undereducated rube like myself could learn?"
- Of course! Demolition Science should be for everyone! Let me show you a thing or two.
+ "Why not?"
- \(Professor Lint spends an hour giddily walking you through his research. You feel like you have an Eye for Architecture now\)
+ "This is fascinating stuff. Thanks, Professor!
    -> architect_explosives

== architect_explosives ==
- Remember, if you see a breakable wall, just Interact with it while holding an Explosive to break it down!  # grant_architect
+ "Will do."
    -> END

== architect_files ==
- Hm, I've never seen such a big building. It looks like retail space, mainly.
+ "Retail space?"
- Yeah. Dozens of shops, all in a single building. The plans keep calling it a "Mall", but that's not a term I'm familiar with.
+ "Me neither."
- Whatever it is, it's never happening. They want to build it in the City Park, but that land is protected from development by an Old Treaty.
+ "How old are we talking about?"
- Old old. Colonial. My colleague, Professor Filings mentioned it when I told her about my research.
+ "Professor Filings?"
- Sure, she's the History Professor here. I'm sure she could tell you more about it.
+ "Thanks, I'll ask her."
    -> END
    
== bootlegger_start ==
- It's so boring down here and all I have is this book and it's just a TERRIBLE read.
+ {not bootlegger_reading} "So why stay down here reading in the dark?"
    -> bootlegger_reading
+ {bootlegger_reading} "Say, you mentioned you install doors for all sorts of people, right?
    -> bootlegger_people
+ "Hold that thought."
    -> END
    
== bootlegger_reading == 
- Boss Chaff says the cops are picking up all his guys on flimsy charges. I'm supposed to stay out of sight until the election's done.
+ "So you work for Chaff?"
- That's the messed up part! I'm an independent contractor! I work for all kinds of people!
+ "But mostly Chaff"
- Yeah yeah, mostly.
+ "What is it you do anyway?"
- Oh. So! You know those cool Crime Doors you see all over the city?
+ "Yeah?"
- I've got nothing to do with those. My line of work is installing the Crime Doors you DON'T see.
+ "You install Hidden Crime Doors."
- Oh sure, they're practically invisible unless you know how to spot them.
+ "Could you teach me to spot them?"
- Eh, why not? I could use the company.
+ "Fantastic."
- \(Ned teaches you tricks of his trade. You feel like you'll be able to spot Hidden Crime Doors when you're close to them now\)  # grant_bootlegger
+ "Thanks Ned. Hope the book gets better."
    -> END
    
== bootlegger_people ==
- Oh sure. Tons of people want Hidden Crime Doors. Business guys, politicians, you name it! Even a guy like yourself could use one.
+ "I'll think about it."
    -> END

    
== climber_start ==
- Sweetwhey! My fellow rodent! Can you help me out on this blessed day?
+ "How's the panhandling business, Mollasses?"
    -> climber_panhandling
+ {player_has(dollar) and not climber_trade and climber_panhandling} Sure, here's a dollar.
    -> climber_trade
+ "I'm a little short just now."
    -> END
    
== climber_panhandling ==
- Not great, but it's a sacrifice I make to stay in touch with my Inner Animal.
+ "You mean, to not work?"
- Oh there's more to it than that! Being in touch with your Inner Animal means doing the things SOCIETY doesn't want you to do anymore.
+ "Such as"
- Back in the wild, we used to climb through burrows and trees all the time! Now you're a weirdo unless you use stairs.
+ "I heard they put an elevator in the PelliCorp building"
- Even worse! SOCIETY only wants us to go to predetermined floors like a bunch of squares!
+ "I think I see what you mean."
- I knew you'd get it, Sweetwhey. Say, can you help me out with a dollar?
+ {player_has(dollar) and not climber_trade} Don't spend it all in one place.
    -> climber_trade
+ "I'm a little short just now."
    -> END

== climber_trade ==
- God bless you, Sweetwhey!  # take_dollar # grant_climb
+ "Can you tell me more about that Inner Animal stuff?"
- Absolutely. When you're in touch with your Inner Animal, you can Scurry Up Tunnels just like in the wild!
+ "Show me."
- \(Mollasses teaches you how to Scurry Up Tunnels. You feel in touch with nature, or maybe you're just a little hungry.\)
+ "That was surprisingly enlightening!"
- Try Scurrying up that Tunnel behind me. It should take you to the roof of your Office!
+ "Thanks, I'll give it a go."
    -> END
    
    
== visionman_start ==
- You're handsome, but I am VERY busy. Why are you {not visionman_tinkering: here? | back?}
+ {not visionman_tinkering} "What's that you're tinkering with?
    -> visionman_tinkering
+ {visionman_tinkering and player_has(lens)} "I found that lens you wanted"
    -> visionman_trade
+ "I was just saying hello, is all"
    -> visionman_hello
    
== visionman_hello ==
- HELLO, handsome rat man. Now let me get back to work.
+ "As you were."
    -> END
    
== visionman_tinkering ==
- \(She swivels her chair around\) So everyone knows that bats can see in the dark.
+ "Right."
- WRONG! We echolocate through the dark! Our vision is actually poor, night or day.
+ "Too bad."
- It IS too bad. So I thought: what if bats COULD see in the dark? Then there wouldn't be any confusion.
+ "But you can already echolocate."
- But I can't actually SEE in the dark, YET. This device I'm working on will make it all possible.
+ "So you'll be able to echolocate /and/ see in the dark?"
- I would, but it won't work because those DOORKNOBS at the univeristy won't lend me their Proprietary Nighttime Telescope Lens..."
+ "You just need them to lend it to you?"
- INCORRECT! I need to grind it up into a series of smaller lenses for personal use.
+ "So they won't let you break their Telescope Lens to help you see in the dark?
- Yes, because bats can't actually SEE in the dark! YET! Are you even listening?
+ "Say some good samaritan brought you the telescope lens. 
- Go on.
+ Could you make him see in the dark too?
- How will that solve the confusion about bats seeing in the dark?
+ "Just in theory."
- In THEORY, yes, the miniaturized lenses could help anyone see in the dark, not just bats, although I don't see why anyone would care.
+ "Stay right there, I'll be back."
    -> END

    
== visionman_trade ==
- You found it?
+ "I found it."
- You FOUND it?
+ "Do you want bats to be able to see in the dark or not?"
- Fine. Thanks for FINDING this lens, it'll just take a moment to miniaturize.
+ "That quick?"
- I'm VERY good at this. \(It's true. She grinds up the lens in record time\).
+ "Can I get one of those too?"
- \(Dr. Husk affixes a small lens to your cornea. You feel like you can see in the dark now\)"
+ "It's a little painful."
- That's normal for experimental eyewear. Now PLEASE leave. # take_lens # grant_darkVision
+ "Yep."
    -> END
    
== audioEngineer_start ==
- {not audioEngineer_offTheRecord: Samson. Are you here to return my camera? | Any updates?}
+ {not audioEngineer_offTheRecord} "You know, I must have misplaced it."
    -> audioEngineer_camera
+ {audioEngineer_camera and architect_files and not audioEngineer_park} "I found some blueprints for a big building going up in The Park."
    -> audioEngineer_park
+ {audioEngineer_park and historian_treaty and not audioEngineer_cityHall} "I talked to a historian who said the Old Treaty is kept at City Hall."
    -> audioEngineer_cityHall
+ {audioEngineer_cityHall and records_missingTreaty and not audioEngineer_records} "I checked the Public Records. The Old Treaty isn't there."
    -> audioEngineer_records
+ {audioEngineer_records and not audioEngineer_evidence and (mayor_evidence or player_has(evidence))} "I found the Old Treaty hidden in the mayor's House!"
    -> audioEngineer_evidence
+ {audioEngineer_evidence and not audioEngineer_saveThePark} "Let's save The Park."
    -> audioEngineer_saveThePark
+ {audioEngineer_saveThePark and player_has(wire) and not audioEngineer_trade} "You needed Copper Wire?"
    -> audioEngineer_trade
+ {audioEngineer_trade and not mayor_record} "What am I doing with this Recording Device again?"
    -> audioEngineer_recordingDevice
+ {mayor_record} "I've got the confession!"
    -> audioEngineer_end
+ "I'll be back in a bit"
    -> END
    
== audioEngineer_end ==
- Samson! We did it!
+ "Shucks, we really did!"
    -> epilogue
    
== epilogue ==
- As predicted, Rebecca's news story finally turned public opinion against Mayor Offal. # modify_sprites
+ \(Next\)
- Gus Crumb won the election with a respectable lead. He wasn't a great mayor, but at least he didn't try to demolish The Park.
+ \(Next\)
- What's more, with the Old Treaty still in place, Pellicle never got his mall idea off the ground.
+ \(Next\)
- Samson Sweetwhey enjoyed a new wave of mild fame, although most people only remember him for the Roquefort Diamond case.
+ \(Next\)
- He and Rebecca Bran remain dear friends. When she's not too busy, they enjoy a nice walk in The Park they saved together.
+ \(Next\)
- THE END
+ \(Thanks for playing!\)
    ->END
    
== audioEngineer_recordingDevice ==
- Find a way to get it past the Metal Detectors into City Hall, then confront the Mayor with the Old Treaty.
+ "Then what?"
- Then bring the recording back here. My paper will run a story on it, sink Offal's re-election campaign, and save The Park!
+ "I'll be back in two shakes of a lamb's tail."
    -> audioEngineer_start

== audioEngineer_saveThePark ==
- I knew you'd come around. There's one small snag, though.
+ "What's the problem?"
- Well, two snags. First, the Recording Device is missing a bunch of Copper Wire.
+ "Did someone steal it?"
- Honestly, I think Dr. Husk forgot to put it in.
+ "Okay, so I'll go get some Copper Wire. What's the other snag."
- They just put a Metal Detector in at City Hall.
+ "Ah."
- And the Recording Device will have a bunch of Copper Wire in it.
+ "Naturally."
- BUT, while working on a story about the new Metal Detectors, I did hear there might be a way to beat them.
+ "Great, how do we do it?"
- You're not gonna like this.
+ "Try me."
- Supposedly, the Black Mask Gang already has a way to get Metal through them.
+ "In other words, the extra security measures don't actually do anything."
- I guess not. You've got a lot of sketchy connections, can you look into it?
+ "Let me ask around."
- Great, and don't forget to bring me some Copper Wire, too!
+ "Come on, I remember."
    -> audioEngineer_start
    
== audioEngineer_evidence ==
- It's not enough by itself, but maybe if you confront The Mayor with the Old Treaty, he'll say something incriminating.
+ "Great, I'll go do that!"
- Hold on, Samson. Without a recording of his confession, nobody will believe it.
+ "So what, we get him to confess in a radio station?"
- Not necessary, Dr. Husk designed a Portable Recording Device for me. If you can get it in to The Mayor's Office, we can save The Park!
+ "Who said anything about saving The Park?"
- You'd let them tear down The Park to build some giant ugly shopping warehouse?
+ "That /would/ be a shame but..."
- But what? You still want to find the blackmailer? How much is Candice paying you?
+ "She isn't a very good negotiator..."
- The Park is an important part of our town, people love it!
+ "Sure."
- Plus, your office is right next door! You want to look out the window and see the side of a /mall/?
+ "Maybe malls look alright. I couldn't really tell from the blueprints."
- Samson. Remember when you cracked the Roquefort Diamond case?
+ "Of course."
- So much press. Everyone loved you. But it didn't make you happy, right?
+ "The last few years could have been better."
- /Because/ at the end of the day, you just helped some rich lady get her necklace back. You didn't care.
+ "..."
- I /know/ you, Samson. You only really care about the little people. Just today, you ran all over town helping them out.
+ "I was doing a job."
- You were out on a wild goose chase. We're still no closer to finding the blackmailer, right?
+ "I guess not."
- And why aren't we any closer? Do you think it might be that you it isn't /really/ important to you at all?
+ "..."
- Help me save The Park, please.
+ "I'll think about it"
    -> END

    
== audioEngineer_records ==
- Somebody must have stashed it away somewhere. If nobody can prove the Old Treaty exists, the construction project can go ahead in The Park!
+ "I'll find the Old Treaty, whatever it takes"
    -> audioEngineer_start
    
== audioEngineer_cityHall ==
- You should go look it up in the Public Records then. Maybe there's some kind of loophole?
+ "Yeah..."
    -> audioEngineer_start
    
== audioEngineer_park ==
- The Park? I mean it's a real shame, but why are you telling me?
+ "Apparently, it's illegal to build anything there, on account of an old colonial Treaty."
- So, The Park is safe then? Why would anyone bother writing up plans to build there?
+ "I don't know yet. I need to keep looking."
    -> audioEngineer_start
    
== audioEngineer_gotoDaughter ==
- So go talk to her. She lives in the Mayor's Mansion on the Left side of town, Then come back and tell me about it, I'm curious.
+ "Right."
    -> END

== audioEngineer_camera ==
- Too bad. Maybe you can buy me the one I saw down at the pawnshop, it's the same model.
+ "Yeah, maybe... So I got a letter from the Mayor's Daughter-"
- Candice Offal? She's a brat, what did she say?
+ {not mayorDaughter_problem} I don't know, I haven't talked to her yet.
    -> audioEngineer_gotoDaughter
+ {mayorDaughter_problem} "Can we talk off the record?"
    -> audioEngineer_offTheRecord
    
== audioEngineer_offTheRecord ==
- Can you talk to a reporter /off the record/ about the Mayor's Daughter just days before he's up for re-election? Sure, why not?
+ "Off the record, I mean it."
- What did she say?
+ "He's being blackmailed."
- What about?
+ "I didn't ask."
- Inspiring detective work, Samson.
+ "But listen, she thinks YOU might be the blackmailer"
- Because I've written exposés on her dad's many, many conflicts of interest? It's not me.
+ "Right. I mean if you had any dirt you'd just publish it."
- If you've got it all figured out, why are you here?
+ "Have you heard of anyone who might blackmail the Mayor?"
- Off the record?
+ "Sure, why not?"
- The word is his opponent, Gus Crumb, is paying for dirt on Offal. It's too flimsy for a news story, but there's something there.
+ "Who's he hiring?"
- Sketchy types, apparently. I'm surprised you didn't get a call.
+ "Flattery won't get you anywhere."
- If it's true, maybe one of these guys actually found something and is trying to double dip.
+ "You mean, they're trying to collect a reward from Crumb /and/ blackmail Offal."
- Could be. I hate to admit it, but you've piqued my interest, Samson. Let me know if you find anything out.
+ "I'll keep digging."
    -> END


== audioEngineer_trade ==
- This is perfect. Let me rig up the Recording Device. # take_wire
+ "Alright"
- Okay, it should be good to go, but listen: you know how I said this is a Portable Recording Device?
+ "Sure..."
- Just keep in mind that there are degrees of portability.
+ "Don't tell me."
    -> audioEngineer_give
    
== audioEngineer_give ==
- \(receive item\) # give_recorder
+ Okay
    -> END

== ceo_start ==
- {not ceo_ticket: Oh good, you're here. Be a lamb and pick up my new luggage for me. | Have you got my luggage yet, Max?}
+ {not ceo_ticket} "Excuse me?"
    -> ceo_ticket
+ {ceo_ticket} "They said it's gonna take a few more days to be ready"
    -> ceo_ready
    
== ceo_ready ==
- Unbelievable. I was really looking forward to carrying around that jumbo suitcase, too.
+ "It's a shame, really."
    -> END

== ceo_ticket ==
- You'll need to show them this ticket, I ordered months in advance.
+ "Sure, I'll see what I can do"
- You /do/ work for me, right? What's your name again?
+ Michael
    -> ceo_name
+ Mitchell
    -> ceo_name
+ Micky
    -> ceo_name

== ceo_name ==
- Ah yes, that's right. Anyway, here's the ticket for my luggage order. Chop chop.
+ "Yes sir."
    -> ceo_give

    
== ceo_give ==
- \(receive item\) # give_ticket
+ Okay
    -> END
    
== mobBoss_start ==
- {not mobBoss_quest: Quite an entrance. You're Samson Sweetwhey, right? Charming to meet you. | What's new, Sweetwhey?}
+ {not mobBoss_quest} "It's a joy to meet such an influential member of the community."
    -> mobBoss_community
+ {mobBoss_quest and player_has(package)} Here's the innocuous parcel you wanted.
    -> mobBoss_trade
+ {mobBoss_quest} Not much.
    -> END

== mobBoss_community ==
- It takes all sorts. Anything in particular I can help you with, or are you just dropping in to visit?
+ {audioEngineer_saveThePark} "How do I beat the Metal Detector?"
    -> mobBoss_quest
+ "Not right now"
    -> END
    
== mobBoss_quest ==
- HA! You're up to no good, I'm sure. I'd be happy to help you with this, if you can do something for me.
+ "What do you have in mind?"
- Yesterday, the cops started picking up all of my associates on ridiculous trumped up charges.
+ "The poor guys."
- It's gotten so bad that I've asked them all to stay out of the public streets until things cool down.
+ "Seems sensible."
- This has created a small problem for me.
+ "Go on."
- One of my associates was scooped up while ferrying a very private and time-sensitive parcel for me.
+ "So the cops have it now?"
- Fortunately, no. He had the sense to stash it away in PelliCorp tower somewhere.
+ "I see."
- Obviously, none of my associates can retrieve it right now. It would consider it a personal favor if you would bring it to me.
+ "And you'll help me beat the Metal Detector?"
- I always repay a personal favor.
+ "Then I'll be back with your parcel soon."
    -> END
    
== mobBoss_trade ==
- Splendid! Here's a special lining you can apply to your briefcase. Anything inside will pass through the Metal Detector just fine now.  # grant_antiMetalDetector  # take_package
+ "Nice doing business with you, Boss."
    -> END

== opponent_start ==
- {not opponent_heard: Greetings, citizen! Can I count on your vote next Tuesday? | \(Crumb does his best to ignore you\)}
+ {opponent_heard} \(Leave\)
    -> END
+ {not opponent_heard} "You know, I think I heard that one before."
    -> opponent_heard

== opponent_heard ==
- You do seem like the worldly type. What can I do for you?
+ "Aren't you busy wrapping up your campaign?"
- I /always/ have time for a future constituent. What's on your mind?
+ "I heard you know some nasty rumors about Mayor Offal. Could you fill me in?"
- Let's see, he's aggressively pro-business, pro-police, and pro-war, but besides those things I can't agree with a single thing in his platform.
+ "Sure sure, but what about his personal life, anything there?"
- What are you, a reporter?
+ "Not exactly. I'm sort of a private detective."
- Well, /detective/, I intend to bring /grace/ back to politics. I'm not interested in mudslinging.
+ "It's just, I heard there's some really juicy dirt around about him that's been getting around."
- Anything specific? If there's any truth to those rumors, we need to publicize them. The voters need to know!
+ I thought you didn't like mudslinging.
- As a detective, you should know it isn't unusual for politicians to secure 'opposition research.'
+ "Are you offering me a job?"
- I was thinking about it, but you're rubbing me the wrong way. Anyway, I've already got it covered.
+ "By who?"
- \(He turns away.\)
+ "Good luck on the campaign, Gus.
    -> END

    
== historian_start ==
- Welcome to my classroom! Are you here to learn about our town's History?
+ {architect_files} "What do you know about an Old Treaty protecting The Park?"
    -> historian_treaty
+ {records_missingTreaty} "I searched the Public Records for the Old Treaty. It's not there!"
    -> historian_missingTreaty
+ "I'm alright for now"
    -> END
    
== historian_treaty ==
- Among other things, it stipulates that The Park must remain a public space.
+ "Forever?"
- Essentially, yes. There are further requirements that it needs to be 'a place for nature' and the like, but they're somewhat vague.
+ "Does that mean it would be illegal to develop on the park?"
- I'm no attorney, but I'd say so.
+ "Where can I find a copy of the Old Treaty?"
- As far as I know there's just the one Official Copy in the Public Records at City Hall...
+ "Thanks, I'll take a look."
    -> END
    
== historian_missingTreaty ==
- That's very troubling. The document is pretty obscure, I don't know of any other copies.
+ "But you remember what it said, right? Can it still be enforced?"
- If nobody can produce an official copy, it seems unlikely, I'm afraid.
+ "Then we need to find it!"
    -> END
    
== records_start ==
- \(You're digging through the Public Records\)
+ {historian_treaty or audioEngineer_cityHall} Search for the Old Treaty.
    -> records_missingTreaty
+ {player_has(coin)} \(Add your Lucky Coin to the Public Records\)
    -> records_coin
+ "Boy, I really don't know what I'm looking for here."
    -> END

== records_coin ==
- "Even I'm not sure why I did that." # take_coin
+ \(Leave\)
    -> END

== records_missingTreaty ==
- \(You painstakingly search through the Public Records, but the Old Treaty is missing!\)
+ "How strange."
    -> END

// environment descriptions
== tutorial_poster ==
- Welcome! Let's do a quick overview of the controls! Press J.
+ Okay
- W and D move left and right. Spacebar jumps.
+ Okay
- By holding down S, you'll Scurry. Scurrying is faster than walking, but you can't jump as high.
+ Okay
- You can press J to Interact. Doors, items, and characters can all be interacted with.
+ Okay
- In dialogue, W and S change your selected dialogue choice, and J confirms it.
+ Okay
- You can press K to Examine. Items and some pieces of scenery can be examined.
+ Okay
- You can press I to open you inventory. WASD moves the inventory cursor. J selects and deselects items.
+ Okay
- You can press Spacebar to rotate a selected item in your inventory.
+ Okay
- Finally, pressing I will close your inventory. Any items placed out of bounds will appear red and be dropped.
+ Okay.
- It's a lot. On itch.io, these controls will also appear in the description below for reference!
+ Phew.
    -> END

== tutorial_letter ==
- \(Someone slipped a letter under your door while you were napping.\)
+ \(Read it\)
- 'Detective, I need to consult you on a sensitive matter. You can find me at the Mayor's Mansion, all the way on the Left side of town'
+ "Signed?"
- 'Candice Offal'
+ "Sounds like a case to me!"
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
- A rope. Interact with a Hook Symbol while this is in your inventory to extend a Zipline.
+ Okay
    -> END

== explosive_desc ==
- An explosive. Interact with a Weak Wall while this is in your inventory to break it.
+ Okay
    -> END
    
== lessonPlan_desc ==
- A university lesson plan. Seems to be about probability.
+ Okay
    -> END
    
== evidence_desc ==
- An Old Treaty, taken from the Public Records at City Hall.
+ Okay
    -> END

== clownNose_desc ==
- A bright red clown nose. The performer in The Park needs this.
+ Okay
    -> END
    
== lens_desc ==
- A special telescope lens. A scientist might want this.
+ Okay
    -> END
    
== wire_desc ==
- A spool of copper wire. Good for radios and other audio equipment.
+ Okay
    -> END
    
== package_desc ==
- A suspicious parcel. Could be anything in there.
+ Okay
    -> END
    
== ticket_desc ==
- A ticket from Fancifold Luggage Suppliers. A new briefcase should be ready for pickup today.
+ Okay
    -> END
    
== recorder_desc ==
- A portable recording device, depending on your definition of "portable."
+ Okay
    -> END
    
== files_desc ==
- Some kind of architectural plans or blueprints. An architect code help you understand them.
+ Okay
    -> END