#!/bin/bash

informe_tex="informe.tex"
presentacion_tex="presentacion.tex"
informe_pdf="informe.pdf"
presentacion_pdf="presentacion.pdf"

function clean {
cd "../informe"
rm -f *.log *.gz *.aux *.nav *.snm *.toc
cd "../presentacion"
rm -f *.log *.gz *.aux *.nav *.snm *.toc
echo "todo limpio"
}

function run {
cd ".."
make dev
}

function report {
cd "../informe"
 pdflatex "$informe_tex"
}

function slides {
cd "../presentacion"
 pdflatex "$presentacion_tex"
}

function show_reporte {
cd "../informe"
if [ ! -f "$informe_pdf" ]; then
report
fi
if [ "$1" == "" ]; then
if [ "$OSTYPE" == "linux-gnu" ]; then 
xdg-open "$informe_pdf"
elif [ "$OSTYPE" == "darwin" ]; then
open "$informe_pdf"
else
start "$informe_pdf"
fi
else
"$1" "$informe_pdf"
fi
}

function show_slidess {
cd "../presentacion"
if [ ! -f "$presentacion_pdf" ]; then
slides
fi
if [ "$1" == "" ]; then
if [ "$OSTYPE" == "linux-gnu" ]; then 
xdg-open "$presentacion_pdf"
elif [ "$OSTYPE" == "darwin" ]; then
open "$presentacion_pdf"
else
start "$presentacion_pdf"
fi
else
"$1" "$presentacion_pdf"
fi
}

case $1 in  
clean) clean ;;
run) run ;;
report) report ;;
slides) slides ;;
show_report) show_reporte $2 ;;
show_slides) show_slidess $2 ;;
*) echo "invalido" exit 
esac
exit
