#/bin/python3

from pathlib import Path
import shutil
import yaml

for path in Path('./Examples').rglob('build.yml'):
    with open(path, 'r') as file:
        build = yaml.safe_load(file)
        if 'Example' not in build or 'src' not in build['Example'] or 'target' not in build['Example']:
            print(f"Could not build example {path}")
            continue
        src = f"{path.parents[0]}/{build['Example']['src']}"
        target = f"../{build['Example']['target']}"
        print(f"Building {src} => {target}")
        shutil.copyfile(src, target)