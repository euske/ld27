#!/usr/bin/env python
import sys
import pygame
import random

pygame.font.init()
color = (255,255,255,255)
img = pygame.Surface((100, 1000), pygame.SRCALPHA)
font = pygame.font.Font('/usr/share/fonts/TTF/Vera.ttf', 12)
img.fill((255,255,255,255))
for i in xrange(5):
    x = i*20+10
    img.fill((128,128,128,255), (x, 0, 1, 1000))
for i in xrange(10):
    y = i*100+40
    text = font.render("LD %d" % random.randrange(100), 0, (0,0,255,255))
    (w,h)=text.get_size()
    img.blit(text, ((100-w)/2, y))
pygame.image.save(img, 'floor.png')
