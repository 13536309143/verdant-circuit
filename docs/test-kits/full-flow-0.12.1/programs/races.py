challenges = [(Leaderboards.Hay, 'race_hay'), (Leaderboards.Hay_Single, 'race_hay'), (Leaderboards.Wood, 'race_wood'), (Leaderboards.Wood_Single, 'race_wood'), (Leaderboards.Carrots, 'race_carrots'), (Leaderboards.Carrots_Single, 'race_carrots'), (Leaderboards.Pumpkins, 'race_pumpkins'), (Leaderboards.Pumpkins_Single, 'race_pumpkins'), (Leaderboards.Sunflowers, 'race_sunflowers'), (Leaderboards.Sunflowers_Single, 'race_sunflowers'), (Leaderboards.Cactus, 'race_cactus'), (Leaderboards.Cactus_Single, 'race_cactus'), (Leaderboards.Maze, 'race_maze'), (Leaderboards.Maze_Single, 'race_maze'), (Leaderboards.Dinosaur, 'race_dinosaur'), (Leaderboards.Fastest_Reset, 'race_reset')]
for category,program in challenges:
    print("challenge_start",category)
    leaderboard_run(category,program,64)
    print("challenge_passed",category)
print("all_16_challenges_finished",True)
