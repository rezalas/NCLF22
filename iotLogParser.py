#
# By Paul McDowell
# developed for NCL 10/22/22

#
# Scenario: This processes a custom log format where the following bits represent
# an ID, a function number, and a value:
# III|FF|VVV
#

from ast import literal_eval

lines = list[str]
with open('minimal.hex.log') as f:
    lines = f.readlines()
    f.close()

newLines = []
# converts the hex form of the binary to space separated integer values 
for line in lines:
    newVal = bin(literal_eval(line))[2:].zfill(8)
    newLines.append("{0} {1} {2}\n".format(int(newVal[0:3],2),int(newVal[3:5],2),int(newVal[5:],2)))

# saves the processed file for later use
with open ('processed.log','x') as f:
    f.writelines(newLines)
    f.close()

# Q1 and Q2
# finds the least passed value of function #3.

func3Vals = []

# if you need to open the processed file vs the live data above, use this
# lines = []
# with open ('processed.log') as f:
#     lines = f.readlines()
#     f.close()

for line in newLines:
    if line[2] == '3':
        func3Vals.append(int(line[4]));

c, least = len(func3Vals), 0
for x in func3Vals:
    if func3Vals.count(x) <= c :
        c = func3Vals.count(x)
        least = x

print("{0} seen {1} times ".format(least, c))

# Q 3 
# Provides the average of all values seen by function #2
lines = []
func2Vals = []
func2Total = 0
with open ('processed.log') as f:
    lines = f.readlines()
    f.close()

for line in newLines:
    if line[2] == '2':
        cVal = int(line[4])
        func2Vals.append(cVal)
        func2Total += cVal

average = func2Total / len(func2Vals)
print(average)
