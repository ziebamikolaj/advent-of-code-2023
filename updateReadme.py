import os

readme_template = """
# 🎄 Advent of Code Progress 🌟

**Welcome to my Advent of Code repository!** Dive into my coding adventures with these fun and challenging puzzles.

## 🚀 Progress Overview

📊 Progress: **{} out of 25 challenges completed!**

![Progress](https://progress-bar.dev/{}/?title=Completed&width=300)

| Day | Status | Solution |
|:---:|:------:|:--------:|
{}

## About Advent of Code

[Advent of Code](https://adventofcode.com/) is a delightful series of coding challenges released each December. It's a fantastic way to sharpen coding skills and celebrate the holiday season with the coding community.

## Connect with Me

Follow my coding journey on [GitHub](https://github.com/ziebamikolaj). Happy coding!

"""

# Generate the progress checklist
completed_count = 0
progress_rows = []
for i in range(1, 26):  # Assuming 25 days in Advent of Code
    day_file = f"Day{i}/Day{i}.cs"
    file_exists = os.path.isfile(day_file)
    status_badge = "![Completed](https://img.shields.io/badge/Day%20{}-Completed-green)".format(i) if file_exists else "![Incomplete](https://img.shields.io/badge/Day%20{}-Incomplete-red)".format(i)
    solution_link = f"[🔗 Solution](https://github.com/ziebamikolaj/advent-of-code-2023/blob/main/{day_file})" if file_exists else "⏳ In Progress"
    
    progress_rows.append(f"| Day {i} | {status_badge} | {solution_link} |")
    completed_count += int(file_exists)

# Update the README
with open("README.md", "w") as readme_file:
    progress_percentage = int((completed_count / 25) * 100)
    readme_file.write(readme_template.format(completed_count, progress_percentage, "\n".join(progress_rows)))
