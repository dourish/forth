32 constant bl

 \ The fast wrapping requires dimensions that are powers of 2.
 1 6 lshift constant w \ 64
 1 4 lshift constant h \ 16
 
 : rows    w * 2* ;
 1 rows constant row
 h rows constant size
 
 create world size allot
 world   value old
 old w + value new
 
 variable gens
 : clear  world size erase     0 gens ! ;
 : age  new old to new to old  1 gens +! ;
 
 : col+  1+ ;
 : col-  1- dup w and + ; \ avoid borrow into row
 : row+  row + ;
 : row-  row - ;
 : wrap ( i -- i ) [ size w - 1- ] literal and ;
 : w@ ( i -- 0/1 ) wrap old + c@ ;
 : w! ( 0/1 i -- ) wrap old + c! ;
 
 : foreachrow ( xt -- )
   size 0 do  i over execute  row +loop drop ;
 
 : showrow ( i -- ) cr
   old + w over + swap do i c@ if [char] * else bl then emit loop ;
 : show  ['] showrow foreachrow  cr ." Generation " gens @ . ;
 
 : sum-neighbors ( i -- i n )
   dup  col- row- w@
   over      row- w@ +
   over col+ row- w@ +
   over col-      w@ +
   over col+      w@ +
   over col- row+ w@ +
   over      row+ w@ +
   over col+ row+ w@ + ;
 : gencell ( i -- )
   sum-neighbors  over old + c@
   or 3 = 1 and   swap new + c! ;
 : genrow ( i -- )
   w over + swap do i gencell loop ;
 : gen  ['] genrow foreachrow  age ;
 
 : life  begin gen 0 0 at-xy show key? until ;
 
 \ patterns
 char | constant '|'
 : pat ( i addr len -- )
   rot dup 2swap  over + swap do
     i c@ '|' = if drop row+ dup else
     i c@ bl  = 1+ over w!  col+ then
   loop 2drop ;
 
 : blinker s" ***" pat ;
 : toad s" ***| ***" pat ;
 : pentomino s" **| **| *" pat ;
 : pi s" **| **|**" pat ;
 : glider s"  *|  *|***" pat ;
 : pulsar s" *****|*   *" pat ;
 : ship s"  ****|*   *|    *|   *" pat ;
 : pentadecathalon s" **********" pat ;
 : clock s"  *|  **|**|  *" pat ;
