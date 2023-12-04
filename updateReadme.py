import os

readme_template = """
# 🎄 Advent of Code Progress 🌟

**Welcome to my Advent of Code repository!** Here you'll find my solutions to the annual Advent of Code programming challenges. Advent of Code is a series of small programming puzzles for a variety of skill sets and skill levels in any programming language you like.

## 🚀 Progress Overview

I have completed **{} out of 25** challenges so far!

| Day | Status |
|-----|--------|
{}

## Stay Tuned!

For more updates, follow me on [GitHub](https://github.com/ziebamikolaj). Happy coding!

"""

# Generate the progress checklist
completed_count = 0
progress_rows = []
for i in range(1, 26):  # Assuming 25 days in Advent of Code
    day_file = f"Day{i}.cs"
    if os.path.isfile(day_file):
        progress_rows.append(f"| Day {i} | ![Completed](https://img.shields.io/badge/-Completed-green) |")
        completed_count += 1
    else:
        progress_rows.append(f"| Day {i} | ![Incomplete](https://img.shields.io/badge/-Incomplete-red) |")

# Update the README
with open("README.md", "w") as readme_file:
    readme_file.write(readme_template.format(completed_count, "\n".join(progress_rows)))
