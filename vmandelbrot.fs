
\ ASCII mandelbrot based on a BASIC program
\ then updated for RA8875
\ I thik I can make this work mainly by changing .char, which outputs
\ a single character based on the value on the stack.
\ will also need to keep track of my horizontal and vertial position

variable hpos
variable vpos

100 constant hoffset
0 constant voffset
6 constant hsize
5 constant vsize

20 constant maxiter
-39 constant minval
40 constant maxval
20 5 lshift constant rescale
rescale 4 * constant s_escape
variable creal
variable cimag
variable zreal
variable zimag
variable count

: zr_sq zreal @ dup rescale */ ;
: zi_sq zimag @ dup rescale */ ;

create colors
    15 , 00 , 31 ,
    31 , 00 , 31 ,
    31 , 00 , 20 ,
    31 , 00 , 08 ,
    31 , 00 , 00 ,
    31 , 16 , 00 ,
    31 , 24 , 00 ,
    31 , 34 , 00 ,
    31 , 37 , 00 ,
    20 , 47 , 00 ,
    16 , 47 , 00 ,
    08 , 47 , 00 ,
    04 , 47 , 00 ,
    00 , 47 , 00 ,
    00 , 47 , 16 ,
    00 , 47 , 31 ,
    00 , 24 , 31 ,
    00 , 00 , 31 ,

: setcolorforvalue
   3 * dup dup >r >r
   cells colors + @
   r> 1+ cells colors + @
   r> 1+ 1+ cells colors + @
   fgrgb ;
   
: tocolor
   dup 3 < if drop magenta else
   dup 6 < if drop red  else
   dup 9 < if drop orange else
   dup 12 < if drop yellow else
   dup 15 < if drop green else
   18 < if cyan else
   black then then then then then then ;

: .char dup 18 > if
      1 hpos +!
      drop
    else
      \ select color from 0 to 21 (black for highest values)
      \ tocolor fg
      setcolorforvalue
      hpos @ hsize * hoffset +  \ convert hpos to x coordinate
      vpos @ vsize * voffset +  \ and for vpos to y
      2dup vsize + >r hsize + r> fillrect
      1 hpos +!
     then ;

: escapes? s_escape > ;

: count_and_test?
    count @ 1+ dup count !
    maxiter > ;

: init_vars
    5 lshift dup creal ! zreal !
    5 lshift dup cimag ! zimag !
    1 count ! ;


: doescape
    zr_sq zi_sq 2dup +
    escapes? if 2drop true
    else
      - creal @ +
      zreal @ zimag @ rescale */ 1 lshift
     cimag @ + zimag ! zreal !
     count_and_test? then ;

: docell
    init_vars
    begin doescape until count @ .char ;

: dorow maxval minval do dup i docell loop
    drop ;

: vmandelbrot
    cr
    0 vpos ! 0 hpos !
    maxval minval do
    i dorow 1 vpos +! 0 hpos !
    loop white fg ;



