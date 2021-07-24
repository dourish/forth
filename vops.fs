( video operations on RA8875 using v! and v@ )

$0000 constant black
$001f constant blue
$f800 constant red
$07e0 constant green
$07ff constant cyan
$f81f constant magenta
$fee0 constant yellow
$ffff constant white

( color -- r g b )
: color>bytes
    dup 11 rshift $1f and swap  \ red
    dup 5 rshift $3f and swap   \ green
    $1f and ;                   \ blue

 : fgrgb  $65 v! $64 v! $63 v! ;
 : bgrgb  $62 v! $61 v! $60 v! ;


( set foreground color ) 
: fg  color>bytes fgrgb ;

( set background color )
: bg  color>bytes  bgrgb ;

: setrect ( startx starty endx endy -- )
   dup $ff00 and 8 rshift $98 v! $00ff and $97 v!
   dup $ff00 and 8 rshift $96 v! $00ff and $95 v!
   dup $ff00 and 8 rshift $94 v! $00ff and $93 v!
   dup $ff00 and 8 rshift $92 v! $00ff and $91 v!
 ;

: fillrect ( startx starty endx endy -- )
   setrect
   \ draw with parameters
   $10 $90 v!                    \ stop (necessary?)
   $b0 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
 ;   

: drawrect ( startx starty endx endy -- )
   setrect
   \ draw with parameters
   $10 $90 v!                    \ stop (necessary?)
   $90 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
 ;   

: drawline ( startx starty endx endy -- )
   setrect
   \ draw with parameters
   $10 $90 v!                    \ stop (necessary?)
   $80 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
 ;


: setupcircle ( centerx centery radius -- )
   $9d v!                          \ set radius
   dup $ff00 and 8 rshift $9c v!   \ centery MSB
   $ff and $9b v!                  \ centery LSB
   dup $ff00 and 8 rshift $9a v!   \ centerx MSB
   $ff and $99 v!                  \ centerx LSB
 ;    
    
: fillcircle ( centerx centery radius -- )
   setupcircle
   $0 $90 v!
   $60 $90 v!
   begin $90 v@ 64 and while repeat     \ loop until busy signal is clear
 ;    

: drawcircle ( centerx centery radius -- )
   setupcircle
   $0 $90 v!
   $40 $90 v!
   begin $90 v@ 64 and while repeat     \ loop until busy signal is clear
 ;    

: setuptriangle ( x1 y1 x2 y2 x3 y3 -- )
   dup $ff00 and 8 rshift $98 v! $00ff and $97 v!
   dup $ff00 and 8 rshift $96 v! $00ff and $95 v!
   dup $ff00 and 8 rshift $94 v! $00ff and $93 v!
   dup $ff00 and 8 rshift $92 v! $00ff and $91 v!
   dup $ff00 and 8 rshift $ac v! $00ff and $ab v!
   dup $ff00 and 8 rshift $aa v! $00ff and $a9 v!
 ;   

: filltriangle ( x1 y1 x2 y2 x3 y3 -- )
   setuptriangle
   \ draw with parameters
   $11 $90 v!                    \ stop (necessary?)
   $a1 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
 ;   

: drawtriangle ( x1 y1 x2 y2 x3 y3 -- )
   setuptriangle
   \ draw with parameters
   $11 $90 v!                    \ stop (necessary?)
   $81 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
 ;   



: page
   \ $63 v@ $64 v@ $65 v@         \ save foreground color
   \ 0 $63 v! 0 $64 v! 0 $65 v!   \ set color to black
   0 fg

   \ set up draw coordinates
   0 $91 v! 0 $92 v! 0 $93 v! 0 $94 v!         \ start at 0, 0
   $1f $95 v! $03 $96 v! $df $97 v! $1 $98 v!  \ end at 799,479

   \ draw with parameters
   $10 $90 v!                    \ stop (necessary?)
   $b0 $90 v!                    \ start
   begin $90 v@ 128 and while repeat     \ loop until busy signal is clear
   
   \ set text position registers
   0 $2a v! 0 $2b v! 0 $2c v! 0 $2d v!
   \ reset FORTH's idea of the text position
   0 $0a c!  0 $ab c!           \ setting VROW and VCOL
   
   \ restore background color
   \ $65 v! $ 64 v! $ 63 v!      ;
   $ffff fg ;

: demolines
   31 0 do
    100 100 i 10 * + dup 400 drawline loop ;

: democolors
   32 0 do
     i 0 0 fgrgb
     400 i 10 * + dup 9 + 100 swap 200 fillrect loop
   32 0 do
     0 i 2 * 0 fgrgb
     400 i 10 * + dup 9 + 200 swap 300 fillrect loop
   32 0 do
     0 0 i fgrgb
     400 i 10 * + dup 9 + 300 swap 400 fillrect loop
   white fg
;

( field of circles in random colors )

: democircles
   12 0 do 20 0 do
     \ i j + $2f j 3 * - j 2 * fgrgb
     $1f i 3 * - 0 max    \ red
     j 4 *                \ green
     i 10 - 0 max 3 *     \ blue
     fgrgb
     i 40 * 20 +
     j 40 * 20 +
     18 fillcircle
   loop loop
   white fg
;
