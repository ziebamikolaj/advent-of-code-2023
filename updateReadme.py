import os

readme_template = """
# 🎄 Advent of Code Progress 🌟

**Welcome to my Advent of Code repository!** Explore my journey through the Advent of Code challenges, a series of fun and engaging programming puzzles.

## 🚀 Progress Overview

I have completed **{} out of 25** challenges! 🎉

![Progress](https://progress-bar.dev/{}/{},title=Completed)

| Day | Status | Solution |
|:---:|:------:|:--------:|
{}

## About Advent of Code

[Advent of Code](https://adventofcode.com/) is an exciting series of Christmas-themed programming challenges. It's a fantastic way to improve coding skills and engage with the coding community.

## Connect with Me

Stay updated with my coding adventures on [GitHub](https://github.com/ziebamikolaj). Happy coding!

"""

# Generate the progress checklist
completed_count = 0
progress_rows = []
for i in range(1, 26):  # Assuming 25 days in Advent of Code
    day_file = f"Day{i}.cs"
    if os.path.isfile(day_file):
        status_icon = "🟢"
        status_link = f"[View Solution](https://github.com/ziebamikolaj/advent-of-code/blob/main/{day_file})"
        completed_count += 1
    else:
        status_icon = "🔴"
        status_link = "Not Started"
    
    progress_rows.append(f"| Day {i} | {status_icon} | {status_link} |")

# Update the README
with open("README.md", "w") as readme_file:
    progress_percentage = (completed_count / 25) * 100
    readme_file.write(readme_template.format(completed_count, progress_percentage, 100, "\n".join(progress_rows)))
