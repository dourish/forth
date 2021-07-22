
\ ASCII mandelbrot based on a BASIC program
\ then updated for RA8875
\ In ASCII version, .char outputs a single character. In video version,
\ it draws a small colored block.

100 constant hoffset
0 constant voffset
6 constant hsize
5 constant vsize

$fc40 constant orange
$87e0 constant chartreuse

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

variable hpos
variable vpos
0 hpos !
0 vpos !


: zr_sq zreal @ dup rescale */ ;
: zi_sq zimag @ dup rescale */ ;

: tocolor
   dup 3 < if drop magenta else
   dup 6 < if drop red  else
   dup 8 < if drop orange else
   dup 10 < if drop yellow else
   dup 12 < if drop chartreuse else
   dup 14 < if drop green else
   dup 16 < if drop cyan else
   18 < if blue else
   black then then then then then then ;

: .char dup 18 > if
      1 hpos +!
      drop
    else
      \ select color from 0 to 21 (black for highest values)
      tocolor fg
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
    maxval minval do
    i dorow 1 vpos +! 0 hpos !
    loop white fg ;



