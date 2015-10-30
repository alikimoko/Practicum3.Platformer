convert -size 70x70 canvas:none -fill red -draw "circle 35,35 10,30" red-circle.png
convert -size 70x70 canvas:none -draw "circle 35,35 35,20" -negate -channel A -gaussian-blur 0x8 white-highlight.png
composite -compose atop -geometry -13-17 white-highlight.png red-circle.png red-ball.png
convert ( red-ball.png -trim ) -resize 30x30 +repage red-ball.png
convert red-ball.png ( ( red-ball.png -background "#00000000" -rotate 24 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 48 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 72 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 96 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 120 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 144 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 168 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 192 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 216 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 240 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 264 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 288 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 312 ) -fuzz 5%% -trim +repage ) +append ball-strip.png
convert ball-strip.png ( ( red-ball.png -background "#00000000" -rotate 336 ) -fuzz 5%% -trim +repage ) +append ball-strip.png