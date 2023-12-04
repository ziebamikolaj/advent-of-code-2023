import os

readme_template = """
# Advent of Code Progress

My progress for Advent of Code:

{}

"""

# Generate the progress checklist
progress = []
for i in range(1, 26):  # Assuming 25 days in Advent of Code
    day_file = f"Day{i}.cs"
    if os.path.isfile(day_file):
        progress.append(f"- [x] Day {i}")
    else:
        progress.append(f"- [ ] Day {i}")

# Update the README
with open("README.md", "w") as readme_file:
    readme_file.write(readme_template.format("\n".join(progress)))
