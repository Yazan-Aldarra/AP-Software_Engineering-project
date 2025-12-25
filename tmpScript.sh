#!/bin/bash
classes=(Running Standing Attacking Dying Falling TakingDamage)
for class in "${classes[@]}"; do
	dotnet new class -n "${class}State" -o States
done
