# mahjong_shanten_calculator
 Calculate shanten of hand with recursion.

Script is based on C#, which can be used on unity.



## Usage

Call a `CalculateShaten` with `int` array.

int value should be below.

| 1m   | 2m    | 3m   | 4m    | 5m    | 6m    | 7m   | 8m   | 9m   |
| ---- | ----- | ---- | ----- | ----- | ----- | ---- | ---- | ---- |
| 0    | 1     | 2    | 3     | 4     | 5     | 6    | 7    | 8    |
| 1p   | 2p    | 3p   | 4p    | 5p    | 6p    | 7p   | 8p   | 9p   |
| 9    | 10    | 11   | 12    | 13    | 14    | 15   | 16   | 17   |
| 1s   | 2s    | 3s   | 4s    | 5s    | 6s    | 7s   | 8s   | 9s   |
| 18   | 19    | 20   | 21    | 22    | 23    | 24   | 25   | 26   |
| east | south | west | north | shiro | hatsu | chu  |      |      |
| 27   | 28    | 29   | 30    | 31    | 32    | 33   |      |      |

Notation

- 1m : 1 man (만)
- 5p : 5 pin (통)
- 8s : 8 sou (삭)

### Returns

shanten count.

- `0`: tenpai
- `1~`: shanten count
- `negative` : maybe completed hand

## Caution

Hand with more than 13, and flush(청일색, 清一色) will cost time to compute.

- hand with 13 : about 1s
- hand with 14 : about 7~8s

It can optimized a bit, but I'll left this as it at current position