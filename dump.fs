: dumpline ( addr -- )
   cr dup 4 .r #32 emit
   #8 #0 do dup i + c@ 3 .r loop #32 emit
   #16 #8 do dup i + c@ 3 .r loop #32 emit
   32 emit 32 emit [char] | emit
   #8 #0 do dup i + c@
     dup #32 < if [char] . emit drop else
       dup #122 > if [char] . emit drop else emit
     then then loop
   #16 #8 do dup i + c@
     dup #32 < if [char] . emit drop else
       dup #122 > if [char] . emit drop else emit
     then then loop [char] | emit
   drop ;


: dump ( baseaddr count -- )
   base @ >r hex    \ save base, switch to hex
   15 + 16 / 0 do
     dup i 16 * + dumpline loop drop
   r> base ! ;      \ restore base on exit
